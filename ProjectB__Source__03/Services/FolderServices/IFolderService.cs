using Microsoft.AspNetCore.Mvc;
using ProjectB.Models.DTOS;
using StageTest.Models.ContainerFoldersModels;
using StageTest.Models.FolderModels;

namespace StageTest.Services.FolderServices
{
    public interface IFolderService
    {
        Task<ContainersFolder> CreateContainerFolder(CreateFolder createFolderDTO);
        Task<List<OutputContainersFoldersDto>> GetAllContainersFolders();
        Task<List<OutputContainersFoldersDto>> GetAllContainersFoldersByParentId(string idParent);
        Task<ContainersFolder> GetContainerFolder(string id);
        Task<string> GetContainerFolderTitle(string id);
        Task<string> GetContainerFolderTitleDynamic(string id);
        Task<OutPutFolderContainersLines> GetAllContainersLinesByFolder(string idFolder);
        Task DeleteContainerFolderAndChildren(string idFolder);
        Task<List<ContainersFolder>> GetAllContainersFoldersParent();
    }
}
