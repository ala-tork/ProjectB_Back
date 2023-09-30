using ProjectB__Target__01.Models.ProjectModels;
using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Models.ProjectVersionModels;

namespace ProjectB__Target__01.Services.ProjectServices
{
    public interface IProjectService
    {
        Task<Project> CreateProject(CreateProject createProject);
        Task<List<PrototypeVersionInfo>> GetPrototypeVersions(string idProject);

        Task<ProjectInfo> GetPrototypeById(string idPrototype);
        Task<Project> CreateProjectFromPrototype(string idprototype);
        Task<Project> DeleteProject(string idProject);
        Task<Project> UpdateoldesProjectVerions(string idProject);

        Task<Project> GetProjectById(string idProject);
    }
}
