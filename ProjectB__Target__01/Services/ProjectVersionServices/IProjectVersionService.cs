using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Models.ProjectVersionModels;

namespace ProjectB__Target__01.Services.ProjectVersionServices
{
    public interface IProjectVersionService
    {
        Task<ProjectsVersion> CreateProjectVersion(CreateProjectVersion createProjectVersion);
        Task<List<FolderInformations>> GetProjectFoldersinfo(string idProjectVersion);

        Task<PrototypeVersionInfo> GetPrototypeVersionByIdFromSource(string idProjectVersionId);
        Task<ProjectsVersion> DeleteProjectVersion(string idProjectVersionId);
    }
}
