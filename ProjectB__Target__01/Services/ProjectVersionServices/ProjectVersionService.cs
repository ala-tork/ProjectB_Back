using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectB__Target__01.Models;
using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Models.ProjectVersionModels;
using ProjectB__Target__01.Services.ProjectsFoldersServices;

namespace ProjectB__Target__01.Services.ProjectVersionServices
{
    public class ProjectVersionService : IProjectVersionService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IProjectFolderService _projectFolderService;

        public ProjectVersionService(ApplicationDbContext context, IMapper mapper,IProjectFolderService projectFolderService)
        {
            _context = context;
            _httpClient = new HttpClient();

            _mapper = mapper;
            _projectFolderService = projectFolderService;
        }

        private readonly string GetContainersFoldersByIdPrototypeVersion = "https://localhost:7047/api/PrototypeVersion/containersfoldersParent/";
        private readonly string GetPrototypeVersionById = "https://localhost:7047/api/PrototypeVersion/";

        public async Task<ProjectsVersion> CreateProjectVersion(CreateProjectVersion createProjectVersion)
        {
            try
            {
                var projectVeriosn = _mapper.Map<ProjectsVersion>(createProjectVersion);
                
                await _context.ProjectsVersions.AddAsync(projectVeriosn);
                await _context.SaveChangesAsync();
                return projectVeriosn;

            }
            catch (Exception ex)
            {
                throw new  Exception(ex.Message);
            }

        }

        public async Task<List<FolderInformations>> GetProjectFoldersinfo(string idProjectVersion)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(GetContainersFoldersByIdPrototypeVersion + idProjectVersion);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<List<FolderInformations>>(responseBody);
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PrototypeVersionInfo> GetPrototypeVersionByIdFromSource(string idProjectVersionId)
        {
            try
            {
                HttpResponseMessage response  = await _httpClient.GetAsync(GetPrototypeVersionById + idProjectVersionId);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<PrototypeVersionInfo>(responseBody);
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProjectsVersion> DeleteProjectVersion(string idProjectVersionId)
        {
            try
            {
                var projectVersion = await _context.ProjectsVersions
                    .FirstOrDefaultAsync(v=>v.IdProjectVersion==Guid.Parse(idProjectVersionId));
                if (projectVersion!=null)
                {
                    var children = await _context.ProjectsVersions
                        .Where(pv=>pv.IdParent==projectVersion.IdProjectVersion)
                        .ToListAsync();
                    if (children.Any())
                    {
                        foreach (var item in children)
                        {
                            await DeleteProjectVersion(item.IdProjectVersion.ToString());
                        }
                    }
                    var folders = await _context.ProjectsFolders
                        .Where(f=>f.IdProjectVersion==projectVersion.IdProjectVersion)
                        .ToListAsync();
                    if (folders.Any()) 
                    {
                        foreach (var folder in folders)
                        {
                            await _projectFolderService.DeleteFolder(folder.IdProjectFolder.ToString());

                        }
                    }
                    _context.ProjectsVersions.Remove(projectVersion);
                    await _context.SaveChangesAsync();
                    return projectVersion;
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
