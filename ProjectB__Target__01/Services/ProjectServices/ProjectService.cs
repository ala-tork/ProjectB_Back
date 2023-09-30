using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectB__Target__01.Models;
using ProjectB__Target__01.Models.ProjectModels;
using ProjectB__Target__01.Models.ProjectVersionModels;
using ProjectB__Target__01.Services.ProjectVersionServices;

namespace ProjectB__Target__01.Services.ProjectServices
{
    public class ProjectService:IProjectService
    {
        private readonly IProjectVersionService _projectVersionService;
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public ProjectService(IProjectVersionService projectVersionService , ApplicationDbContext context, IMapper mapper)
        {
            _projectVersionService = projectVersionService;
            _context = context;
            _httpClient = new HttpClient();
            _mapper = mapper;
        }

        private readonly string GetAllPrototypeURL = "https://localhost:7047/api/Prototype";
        private readonly string GetPrototypeByIdURL = "https://localhost:7047/api/Prototype/";
        private readonly string GetAllPrototypeVerionsByIdPrototype = "https://localhost:7047/api/Prototype/PrototypesVersions/";

        public async Task<Project> CreateProject(CreateProject createProject)
        {
            try
            {
                var project = _mapper.Map<Project>(CreateProject);
                var res = await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();
                return project;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Project> DeleteProject(string idProject)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p=>p.IdProject==Guid.Parse(idProject));
                if(project != null)
                {
                    var projectversions = await _context.ProjectsVersions
                        .Where(pv=>pv.IdProject== project.IdProject)
                        .ToListAsync();
                    foreach( var projectversion in projectversions)
                    {
                        await _projectVersionService.DeleteProjectVersion(projectversion.IdProjectVersion.ToString());                        
                    }

                    _context.Projects.Remove(project);
                    await _context.SaveChangesAsync();
                    return project;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProjectInfo> GetPrototypeById(string idPrototype)
        {
            try
            {
                HttpResponseMessage reponseMessage = await _httpClient.GetAsync(GetPrototypeByIdURL+idPrototype);
                if (reponseMessage.IsSuccessStatusCode)
                {
                    var response = await reponseMessage.Content.ReadAsStringAsync();
                    var prototype = JsonConvert.DeserializeObject<ProjectInfo>(response);
                    return prototype;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PrototypeVersionInfo>> GetPrototypeVersions(string idProject)
        {
            try
            {
                HttpResponseMessage responseMessage = await _httpClient.GetAsync(GetAllPrototypeVerionsByIdPrototype+idProject);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = await responseMessage.Content.ReadAsStringAsync();
                    var prototypeVerions = JsonConvert.DeserializeObject<List<PrototypeVersionInfo>>(response);
                    return prototypeVerions;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Project> CreateProjectFromPrototype(string idprototype)
        {
            try
            {
                var prototype = await GetPrototypeById(idprototype);
                var project = new Project
                {
                    IdProject = prototype.IdPrototype,
                    Title = prototype.Title,
                    Time = DateTime.Now.TimeOfDay,
                    Date = DateTime.Now.Date,
                    ShortId = prototype.ShortId
                };
                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();
                return project;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Project> UpdateoldesProjectVerions(string idProject)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.IdProject == Guid.Parse(idProject));
                if (project != null)
                {
                    var projectversions = await _context.ProjectsVersions
                            .Where(pv => pv.IdProject == Guid.Parse(idProject))
                            .ToListAsync();
                    if (projectversions.Any())
                    {
                        foreach (var version in projectversions)
                        {
                            version.IsLastVersion = false;
                            _context.ProjectsVersions.Update(version);
                        }
                        await _context.SaveChangesAsync();
                    }
                    return project;

                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Project> GetProjectById(string idProject)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.IdProject == Guid.Parse(idProject));
                if(project != null)
                    return project;
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
