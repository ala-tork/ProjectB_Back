using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectB__Target__01.Models;
using ProjectB__Target__01.Models.ProjectsFileModels;
using ProjectB__Target__01.Services.ProjectsFilesLinesServices;
using System.Net.Http;

namespace ProjectB__Target__01.Services.ProjectFilesServices
{
    public class ProjectFileService : IProjectFileService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;
        private readonly IProjectsFileLineService _projectsFileLineService;
        public ProjectFileService(ApplicationDbContext context,IProjectsFileLineService projectsFileLineService)
        {
            _client = new HttpClient();
                _context = context;
            _projectsFileLineService = projectsFileLineService;
        }
       

        private readonly string GetContainerUrl = "https://localhost:7047/api/Conainer";
        private readonly string getContainreTitleUrl = "https://localhost:7047/api/Title/TitleByVariableType/";
        private readonly string getContainerDynamicTitleURL = "https://localhost:7047/api/Conainer/ContainreDynamicTitle/";
        private readonly string getDynamicTitleValues = "https://localhost:7047/api/DataFromJson/var/";
        private readonly string getContainersByContainerFolderURl = "https://localhost:7047/api/Conainer/ContainersByContainerFolder/";
        private readonly string getContainersByIdParentURL = "https://localhost:7047/api/Conainer/ContainersByIdParent/";
       
        public async Task<ProjectsFile> CreateProjectFile(CreateProjectsFile createProjectsFile)
        {
            var projectFile = new ProjectsFile
            {
                IdProjectFile = createProjectsFile.IdProjectFile,
                IdProjectFolder = createProjectsFile.IdProjectFolder,
                Title = createProjectsFile.Title,
                FileCreationDate = DateTime.Now,
                FileCreationTime = DateTime.Now.TimeOfDay,
            };
            await _context.ProjectsFiles.AddAsync(projectFile);
            await _context.SaveChangesAsync();
            return projectFile;
        }

        public async Task<List<ContainerInfo>> GetContainers()
        {
            HttpResponseMessage response = await  _client.GetAsync(GetContainerUrl);
            List<ContainerInfo> containerInfoList = null;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                containerInfoList = JsonConvert.DeserializeObject<List<ContainerInfo>>(responseBody);
                return containerInfoList;  
            }
            return null;

        }

        
        public async Task<string> GetContainerTitle(string id)
        {
            HttpResponseMessage response = await _client.GetAsync(getContainreTitleUrl + id);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                return null;
            }
        }


        public async Task<ProjectsFile> GetProjectsFileById(string id)
        {
            var res = await _context.ProjectsFiles.FirstOrDefaultAsync(f=>f.IdProjectFolder == Guid.Parse(id));
            return res;
        }

        public async Task<List<ProjectsFile>> GetAllProjectFiles()
        {
            var res = await _context.ProjectsFiles.ToListAsync();
            return res;
        }

        public async Task<List<TitleValuesInfo>> GetAllContainersDynamicTitle(string id)
        {
            var dynamicTitle = await GetContainerTitle(id);
            HttpResponseMessage response = await _client.GetAsync(getDynamicTitleValues + dynamicTitle);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var titles = JsonConvert.DeserializeObject<List<TitleValuesInfo>>(responseBody);
                return titles;
            }
            return null;
        }

        public async Task<List<ContainerInfo>> GetAllContainersByIdContainerFolder(string idContainerFolder)
        {
            HttpResponseMessage respons = await  _client.GetAsync(getContainersByContainerFolderURl + idContainerFolder);
            if (respons.IsSuccessStatusCode)
            {
                string responseBody = await respons.Content.ReadAsStringAsync();
                var containers = JsonConvert.DeserializeObject<List<ContainerInfo>>(responseBody);
                return containers;
            }
            return null;
        }

        // Files From Json ANd Containes

        public async Task<string> GetContainerDynamicTitle(string id)
        {
            HttpResponseMessage response = await _client.GetAsync(getContainerDynamicTitleURL + id);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<TitleValuesInfo>> GetAllContainersDynamicTitleFromJson(string id)
        {
            var dynamicTitle = await GetContainerDynamicTitle(id);
            HttpResponseMessage response = await _client.GetAsync(getDynamicTitleValues + dynamicTitle);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var titles = JsonConvert.DeserializeObject<List<TitleValuesInfo>>(responseBody);
                return titles;
            }
            return null;
        }

        public async Task<List<ContainerInfo>> GetContainersByIdParent(string idParent)
        {
            HttpResponseMessage response = await _client.GetAsync(getContainersByIdParentURL + idParent);
            if (response.IsSuccessStatusCode)
            {
                var responsbody = await response.Content.ReadAsStringAsync();
                var containers  = JsonConvert.DeserializeObject<List<ContainerInfo>>(responsbody);
                return containers;
            }
            return null;
        }


        public async Task<List<ProjectsFile>> createDynamicFilesByIdContainer(string idContainerFolder,Guid idFolder)
        {
            var containers = await GetAllContainersByIdContainerFolder(idContainerFolder);
            List<ProjectsFile> files = new List<ProjectsFile>();
            foreach (var container in containers)
            {
                files.AddRange(await CreateFile(container, idFolder));
            }
            return files;
        }

        public async Task<List<ProjectsFile>> CreateFile(ContainerInfo container, Guid idFolder)
        {
            var containerChildren = await GetContainersByIdParent(container.idContainer.ToString());
            List<ProjectsFile> files = new List<ProjectsFile>();

            if (container.title != null)
            {
                var fileToCreate = new CreateProjectsFile
                {
                    IdProjectFolder = idFolder,
                    Title = container.title
                };

                var file = await CreateProjectFile(fileToCreate);
                files.Add(file);
                await _projectsFileLineService.CreateProjectFileLinesForProjectFileUsingJsonFile(container.idContainer,file.IdProjectFile);

                if (containerChildren != null)
                {
                    foreach (var child in containerChildren) 
                    {
                        files.AddRange(await CreateFile(child, idFolder));
                    }
                }
            }
            else if (container.TitleDynamic != null)
            {
                var titlesList = await GetAllContainersDynamicTitleFromJson(container.idContainer);

                foreach (var titles in titlesList)
                {
                    foreach (var title in titles.values)
                    {
                        var fileToCreate = new CreateProjectsFile
                        {
                            IdProjectFolder = idFolder,
                            Title = title
                        };
                        var file = await CreateProjectFile(fileToCreate);
                        files.Add(file);
                        await _projectsFileLineService.CreateProjectFileLinesForProjectFileUsingJsonFile(container.idContainer, file.IdProjectFile);
                    }
                }

                if (containerChildren != null)
                {
                    foreach (var child in containerChildren) 
                    {
                        files.AddRange(await CreateFile(child, idFolder));
                    }
                }
            }

            return files;
        }

        //create files and fileLines using vertical verification

        public async Task<List<ProjectsFile>> createDynamicFilesByIdContainerAndiscertical(string idContainerFolder, Guid idFolder)
        {
            var containers = await GetAllContainersByIdContainerFolder(idContainerFolder);
            List<ProjectsFile> files = new List<ProjectsFile>();
            foreach (var container in containers)
            {
                files.AddRange(await CreateFileAndLines(container, idFolder));
            }
            return files;
        }

        public async Task<List<ProjectsFile>> CreateFileAndLines(ContainerInfo container, Guid idFolder)
        {
            var containerChildren = await GetContainersByIdParent(container.idContainer.ToString());
            List<ProjectsFile> files = new List<ProjectsFile>();

            if (container.title != null)
            {
                var fileToCreate = new CreateProjectsFile
                {
                    IdProjectFolder = idFolder,
                    Title = container.title
                };

                var file = await CreateProjectFile(fileToCreate);
                files.Add(file);
                await _projectsFileLineService.CreateProjectFileLinesForProjectFileUsingisverticalVerification(container.idContainer, file.IdProjectFile);

                if (containerChildren != null)
                {
                    foreach (var child in containerChildren)
                    {
                        files.AddRange(await CreateFile(child, idFolder));
                    }
                }
            }
            else if (container.TitleDynamic != null)
            {
                var titlesList = await GetAllContainersDynamicTitleFromJson(container.idContainer);

                foreach (var titles in titlesList)
                {
                    foreach (var title in titles.values)
                    {
                        var fileToCreate = new CreateProjectsFile
                        {
                            IdProjectFolder = idFolder,
                            Title = title
                        };
                        var file = await CreateProjectFile(fileToCreate);
                        files.Add(file);
                        await _projectsFileLineService.CreateProjectFileLinesForProjectFileUsingisverticalVerification(container.idContainer, file.IdProjectFile);
                    }
                }

                if (containerChildren != null)
                {
                    foreach (var child in containerChildren)
                    {
                        files.AddRange(await CreateFile(child, idFolder));
                    }
                }
            }

            return files;
        }

        public async Task<ProjectsFile> DeleteProjectFile(string idProjectFile)
        {
            try
            {
                var projectFile = await _context.ProjectsFiles
                    .FirstOrDefaultAsync(f => f.IdProjectFile == Guid.Parse(idProjectFile));
                if(projectFile != null)
                {
                    var lines = await _context.ProjectsFilesLines
                                    .Where(fl => fl.IdProjectFile == projectFile.IdProjectFile)
                                    .ToListAsync();
                    foreach (var item in lines)
                    {
                        await _projectsFileLineService.DeleteProjectFileLine(item.IdProjectFileLine.ToString());

                    }
                    _context.ProjectsFiles.Remove(projectFile);
                    await _context.SaveChangesAsync();
                    return projectFile;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
