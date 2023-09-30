using ProjectB__Target__01.Models;
using ProjectB__Target__01.Models.ProjectFileLinesModels;
using ProjectB__Target__01.Models.ProjectsFileModels;
using ProjectB__Target__01.Models.ProjectsFolderModels;

namespace ProjectB__Target__01.Services.ProjectsFoldersServices
{
    public interface IProjectFolderService
    {
        Task<ProjectsFolder> CreateProjecFolder(CreateProjectsFolder projectsFolder);
        Task<List<FolderInformations>> GetContainersFolders();
        Task<List<FolderInformations>> GetContainersFoldersByParentId(string idContainerPrent);
        Task<List<FolderInformations>> GetAllParentContainerFolder();
        Task<string> GetFolderTitle(string id);
        Task<List<TitleValuesInfo>> GetAllDynamicTitle(string DynamicTitle);

        Task<ProjectsFolder> GetFolderById(string id);
        Task<List<ProjectsFolder>> CreateFolders(FolderInformations folderInformations);
        //Task<List<FolderInformations>> CreateFoldersWithIdParent(FolderInformations folderInformations);
        Task<List<FolderInformations>> CreateFoldersWithIdParent(FolderInformations folderInformations,Guid? idparent ,string? level);
        Task<List<FolderInformations>> CreateFoldersAndFiles(FolderInformations folderInformations, Guid? idPrent, string? level);
        Task<List<FolderInformations>> CreateFoldersAndFilesFromProtypeVerson(FolderInformations folderInformations, Guid? idPrent, string? level, Guid idPrototypeversion);


        Task<List<FolderInformations>> CreateFoldersAndFilesAndFileLinesFromProtypeVerson(FolderInformations folderInformations, Guid? idPrent, string? level, Guid idPrototypeversion);

        Task<ProjectsFolder> DeleteFolder(string idFolder);
    }
}
