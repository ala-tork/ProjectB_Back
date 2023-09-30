using ProjectB__Target__01.Models;
using ProjectB__Target__01.Models.ProjectsFileModels;
using System.Runtime.InteropServices;

namespace ProjectB__Target__01.Services.ProjectFilesServices
{
    public interface IProjectFileService
    {
        Task<ProjectsFile> CreateProjectFile(CreateProjectsFile projectsFile);
        Task<List<ContainerInfo>> GetContainers();
        Task<List<ContainerInfo>> GetAllContainersByIdContainerFolder(string idContainerFolder);
        Task<string> GetContainerTitle(string id);
        Task<string> GetContainerDynamicTitle(string id);
        Task<List<TitleValuesInfo>> GetAllContainersDynamicTitleFromJson(string id);
        Task<List<TitleValuesInfo>> GetAllContainersDynamicTitle(string id);
        Task<List<ContainerInfo>> GetContainersByIdParent(string idParent);
        Task<ProjectsFile> GetProjectsFileById(string id);
        Task<List<ProjectsFile>> GetAllProjectFiles();
        Task<List<ProjectsFile>> createDynamicFilesByIdContainer(string idContainer, Guid idFolder);


        Task<List<ProjectsFile>> createDynamicFilesByIdContainerAndiscertical(string idContainerFolder, Guid idFolder);
        Task<List<ProjectsFile>> CreateFileAndLines(ContainerInfo container, Guid idFolder);
        Task<ProjectsFile> DeleteProjectFile(string idProjectFile);


    }
}
