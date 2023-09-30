using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectB__Target__01.Models;
using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Services.ProjectFilesServices;
using System.Net.Http;
using System.Text;

namespace ProjectB__Target__01.Services.ProjectsFoldersServices
{
    public class ProjectFolderService : IProjectFolderService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IProjectFileService _projectFileService;
        public ProjectFolderService(ApplicationDbContext context,IProjectFileService projectFileService)
        {
            _httpClient= new HttpClient();
            _context = context;
            _projectFileService = projectFileService;
        }
        private readonly string GetContainersFoldersUrl = "https://localhost:7047/api/Folder";
        private readonly string getTitleUrl = "https://localhost:7047/api/Folder/FolderTitle/";
        private readonly string GetFolderTitleDynamic = "https://localhost:7047/api/Folder/FolderTitleDynamic/";
        private readonly string getDynamicTitleValues = "https://localhost:7047/api/DataFromJson/var/";
        private readonly string getContainerFolderByIdParentURl = "https://localhost:7047/api/Folder/ContainerFolderByIdParent/";
        private readonly string getAllParentContainerFoldersURL = "https://localhost:7047/api/Folder/AllParentContainersFolders";
        public async Task<ProjectsFolder> CreateProjecFolder(CreateProjectsFolder projectsFolder)
        {
            var folder = new ProjectsFolder
            {
                IdProjectFolder = projectsFolder.IdProjectFolder,
                IdParent = projectsFolder.IdParent,
                Title = projectsFolder.Title,
                
            };
            await _context.ProjectsFolders.AddAsync(folder);
            await _context.SaveChangesAsync();
            return folder;
        }

        public async Task<List<FolderInformations>> GetContainersFolders()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(GetContainersFoldersUrl);
            List<FolderInformations> containerInfoList = null;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                containerInfoList = JsonConvert.DeserializeObject<List<FolderInformations>>(responseBody);
                return containerInfoList;

            }
            return null;
        }

        public async Task<string> GetFolderTitleFromVariable(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(getTitleUrl + id);
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


        public async Task<string> GetFolderTitleFromVariableType(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(GetFolderTitleDynamic + id);
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

        public async  Task<string> GetFolderTitle(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(getTitleUrl+id);
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



        public async Task<ProjectsFolder> GetFolderById(string id)
        {
            var res = await _context.ProjectsFolders.FirstOrDefaultAsync(f=>f.IdProjectFolder == Guid.Parse(id));
            return res;
        }

        public async Task<List<ProjectsFolder>> CreateFolders(FolderInformations folderInformations)
        {
            List<ProjectsFolder> createdFolders =new List<ProjectsFolder>();
            if (folderInformations.Title != null)
            {
                var folder = new CreateProjectsFolder
                {
                    IdProjectFolder = Guid.Parse(folderInformations.IdContainerFolder),
                    Title = folderInformations.Title ?? ""
                };
                if (folderInformations.IdParent != null)
                    folder.IdParent = Guid.Parse(folderInformations.IdParent);

                var createdFolder = await CreateProjecFolder(folder);
                createdFolders.Add(createdFolder);
            }
            else if (folderInformations.TitleDynamic != null)
            {
                var folderTitles = await GetFolderTitleFromVariable(folderInformations.IdContainerFolder);


                        var folder = new CreateProjectsFolder
                        {
                            IdProjectFolder = new Guid(),
                            Title = folderTitles ?? ""
                        };
                        if (folderInformations.IdParent != null)
                            folder.IdParent = Guid.Parse(folderInformations.IdParent);

                        var createdFolder = await CreateProjecFolder(folder);
                        createdFolders.Add(createdFolder);

            }
            return createdFolders;
        }

        public async Task<List<FolderInformations>> GetContainersFoldersByParentId(string idContainerPrent)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(getContainerFolderByIdParentURl + idContainerPrent);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await  response.Content.ReadAsStringAsync();
                var folders = JsonConvert.DeserializeObject<List<FolderInformations>>(responseBody);
                return folders;
            }
            return null;
        }
        public async Task<ProjectsFolder> folderCreation(Guid? idparent, string title)
        {
            try
            {
                var folder = new ProjectsFolder
                {
                    IdProjectFolder = new Guid(),
                    Title = title,
                };
                if(idparent != null)
                    folder.IdParent = idparent;

                await _context.ProjectsFolders.AddAsync(folder);
                await _context.SaveChangesAsync();

                return folder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating folder: {ex}");
                throw ex; 
            }
        }

        public async Task<List<FolderInformations>> GetAllParentContainerFolder()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(getAllParentContainerFoldersURL);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var folders = JsonConvert.DeserializeObject<List<FolderInformations>>(responseBody);
                return folders;
            }
            return null;
        }
        public async Task<List<TitleValuesInfo>> GetAllDynamicTitle(string id)
        {
            var dynamicTitle = await GetFolderTitleFromVariableType(id);
            HttpResponseMessage response = await _httpClient.GetAsync(getDynamicTitleValues + dynamicTitle);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var titles = JsonConvert.DeserializeObject<List<TitleValuesInfo>>(responseBody);
                return titles;
            }
            return null;
        }


        public async Task<List<FolderInformations>> CreateFoldersWithIdParent(FolderInformations folderInformations ,Guid? idPrent,string? level)
        {
            var children = await GetContainersFoldersByParentId(folderInformations.IdContainerFolder);
            var containersFolders = new List<FolderInformations> { folderInformations };
            ProjectsFolder f = null;
            Guid idFolderParent;
            if (folderInformations.Title != null)
            {
                f = await folderCreation(idPrent, folderInformations.Title);
                containersFolders.Add(folderInformations);
            }
            else if (folderInformations.TitleDynamic != null)
            {
                var foldertitles = await GetAllDynamicTitle(folderInformations.IdContainerFolder);
                foreach (var titles in foldertitles)
                {
                    if (level == null)
                    {
                        foreach (var title in titles.values)
                        {
                            f = await folderCreation(idPrent, title);
                            containersFolders.Add(folderInformations);
                            if (children.Count > 0)
                            {
                                foreach (var child in children)
                                {
                                    idFolderParent = f.IdProjectFolder;

                                    await CreateFoldersWithIdParent(child, idFolderParent, title);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (titles.ParentName == level)
                        {
                            foreach (var title in titles.values)
                            {
                                f = await folderCreation(idPrent, title);
                                containersFolders.Add(folderInformations);
                                if (children.Count > 0)
                                {
                                    foreach (var child in children)
                                    {
                                        idFolderParent = f.IdProjectFolder;

                                        await CreateFoldersWithIdParent(child, idFolderParent, title);
                                    }

                                }
                            }
                        }
                    }

                }

            }
            return containersFolders;

        }


        //create folder an files in ones 
        public async Task<List<FolderInformations>> CreateFoldersAndFiles(FolderInformations folderInformations, Guid? idPrent, string? level)
        {
            var children = await GetContainersFoldersByParentId(folderInformations.IdContainerFolder);
            var containersFolders = new List<FolderInformations> { folderInformations };
            ProjectsFolder f = null;
            Guid idFolderParent;
            if (folderInformations.Title != null)
            {
                f = await folderCreation(idPrent, folderInformations.Title);
                await _projectFileService.createDynamicFilesByIdContainer(folderInformations.IdContainerFolder, f.IdProjectFolder);
                containersFolders.Add(folderInformations);
            }
            else if (folderInformations.TitleDynamic != null)
            {
                var foldertitles = await GetAllDynamicTitle(folderInformations.IdContainerFolder);
                foreach (var titles in foldertitles)
                {
                    if (level == null)
                    {
                        foreach (var title in titles.values)
                        {
                            f = await folderCreation(idPrent, title);
                            await _projectFileService.createDynamicFilesByIdContainer(folderInformations.IdContainerFolder, f.IdProjectFolder);
                            containersFolders.Add(folderInformations);
                            if (children.Count > 0)
                            {
                                foreach (var child in children)
                                {
                                    idFolderParent = f.IdProjectFolder;

                                    await CreateFoldersAndFiles(child, idFolderParent, title);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (titles.ParentName == level)
                        {
                            foreach (var title in titles.values)
                            {
                                f = await folderCreation(idPrent, title);
                                containersFolders.Add(folderInformations);
                                if (children.Count > 0)
                                {
                                    foreach (var child in children)
                                    {
                                        idFolderParent = f.IdProjectFolder;

                                        await CreateFoldersAndFiles(child, idFolderParent, title);
                                    }

                                }
                            }
                        }
                    }

                }

            }
            return containersFolders;

        }

        // Create folder and files and lines for prototypeVersion
        public async Task<List<FolderInformations>> CreateFoldersAndFilesFromProtypeVerson(FolderInformations folderInformations, Guid? idPrent, string? level,Guid idPrototypeversion)
        {
            var children = await GetContainersFoldersByParentId(folderInformations.IdContainerFolder);
            var containersFolders = new List<FolderInformations> { folderInformations };
            ProjectsFolder f = null;
            Guid idFolderParent;
            if (folderInformations.Title != null)
            {
                f = await folderCreationWithProjectVersion(idPrent, folderInformations.Title,idPrototypeversion);
                await _projectFileService.createDynamicFilesByIdContainer(folderInformations.IdContainerFolder, f.IdProjectFolder);
                containersFolders.Add(folderInformations);
            }
            else if (folderInformations.TitleDynamic != null)
            {
                var foldertitles = await GetAllDynamicTitle(folderInformations.IdContainerFolder);
                foreach (var titles in foldertitles)
                {
                    if (level == null)
                    {
                        foreach (var title in titles.values)
                        {
                            f = await folderCreationWithProjectVersion(idPrent, title,idPrototypeversion);
                            await _projectFileService.createDynamicFilesByIdContainer(folderInformations.IdContainerFolder, f.IdProjectFolder);
                            containersFolders.Add(folderInformations);
                            if (children.Count > 0)
                            {
                                foreach (var child in children)
                                {
                                    idFolderParent = f.IdProjectFolder;

                                    await CreateFoldersAndFilesFromProtypeVerson(child, idFolderParent, title,idPrototypeversion);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (titles.ParentName == level)
                        {
                            foreach (var title in titles.values)
                            {
                                f = await folderCreationWithProjectVersion(idPrent, title,idPrototypeversion);
                                containersFolders.Add(folderInformations);
                                if (children.Count > 0)
                                {
                                    foreach (var child in children)
                                    {
                                        idFolderParent = f.IdProjectFolder;

                                        await CreateFoldersAndFilesFromProtypeVerson(child, idFolderParent, title,idPrototypeversion);
                                    }

                                }
                            }
                        }
                    }

                }

            }
            return containersFolders;

        }

        public async Task<ProjectsFolder> folderCreationWithProjectVersion(Guid? idparent, string title,Guid idProjectVersion)
        {
            try
            {
                var folder = new ProjectsFolder
                {
                    IdProjectFolder = new Guid(),
                    Title = title,
                    IdProjectVersion=idProjectVersion
                };
                if (idparent != null)
                    folder.IdParent = idparent;

                await _context.ProjectsFolders.AddAsync(folder);
                await _context.SaveChangesAsync();

                return folder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating folder: {ex}");
                throw ex;
            }
        }

        // create Everithing from prototypeVersion and using isvertical verification on lines
        public async Task<List<FolderInformations>> CreateFoldersAndFilesAndFileLinesFromProtypeVerson(FolderInformations folderInformations, Guid? idPrent, string? level, Guid idPrototypeversion)
        {
            var children = await GetContainersFoldersByParentId(folderInformations.IdContainerFolder);
            var containersFolders = new List<FolderInformations> { folderInformations };
            ProjectsFolder f = null;
            Guid idFolderParent;
            if (folderInformations.Title != null)
            {
                f = await folderCreationWithProjectVersion(idPrent, folderInformations.Title, idPrototypeversion);
                await _projectFileService.createDynamicFilesByIdContainerAndiscertical(folderInformations.IdContainerFolder, f.IdProjectFolder);
                containersFolders.Add(folderInformations);
            }
            else if (folderInformations.TitleDynamic != null)
            {
                var foldertitles = await GetAllDynamicTitle(folderInformations.IdContainerFolder);
                foreach (var titles in foldertitles)
                {
                    if (level == null)
                    {
                        foreach (var title in titles.values)
                        {
                            f = await folderCreationWithProjectVersion(idPrent, title, idPrototypeversion);
                            await _projectFileService.createDynamicFilesByIdContainerAndiscertical(folderInformations.IdContainerFolder, f.IdProjectFolder);
                            containersFolders.Add(folderInformations);
                            if (children.Count > 0)
                            {
                                foreach (var child in children)
                                {
                                    idFolderParent = f.IdProjectFolder;

                                    await CreateFoldersAndFilesFromProtypeVerson(child, idFolderParent, title, idPrototypeversion);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (titles.ParentName == level)
                        {
                            foreach (var title in titles.values)
                            {
                                f = await folderCreationWithProjectVersion(idPrent, title, idPrototypeversion);
                                containersFolders.Add(folderInformations);
                                if (children.Count > 0)
                                {
                                    foreach (var child in children)
                                    {
                                        idFolderParent = f.IdProjectFolder;

                                        await CreateFoldersAndFilesFromProtypeVerson(child, idFolderParent, title, idPrototypeversion);
                                    }

                                }
                            }
                        }
                    }

                }

            }
            return containersFolders;

        }

        public async Task<ProjectsFolder> DeleteFolder(string idFolder)
        {
            try
            {
                var folder = await _context.ProjectsFolders.FirstOrDefaultAsync(f => f.IdProjectFolder==Guid.Parse(idFolder));

                if(folder != null)
                {
                    var children  = await _context.ProjectsFolders
                        .Where(f=>f.IdParent==folder.IdProjectFolder)
                        .ToListAsync();
                    if(children.Count > 0)
                    {
                        foreach (var item in children)
                        {
                            await DeleteFolder(item.IdProjectFolder.ToString());
                        }
                    }


                    var files = await _context.ProjectsFiles
                        .Where(f => f.IdProjectFolder == folder.IdProjectFolder)
                        .ToListAsync();
                    if (files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            await _projectFileService.DeleteProjectFile(file.IdProjectFile.ToString());
                        }
                    }
                    _context.ProjectsFolders.Remove(folder);
                    await _context.SaveChangesAsync();
                    return folder;
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
