using ProjectB.Models.PrototypeModels;
using ProjectB.Models.PrototypeVersionModels;
using StageTest.Models.FolderModels;

namespace ProjectB.Services.PrototypeVersionServices
{
    public interface IPrototypeVersionService
    {
        Task<PrototypesVersion> CreatePrototypeVersion(CreatePrototypeVersion createPrototypeVersion);
        Task<List<PrototypesVersion>> GetAllPrototypeVersions();
        Task<PrototypesVersion> GetPrototypeVersionById(string IdProtypeVersion);
        Task<List<ContainersFolder>> GetAllContainersFoldersByIdProtorypeVersion(string IdProtypeVersion);
        Task<List<ContainersFolder>> GetAllContainersFoldersParentByIdProtorypeVersion(string IdProtypeVersion);
        Task<PrototypesVersion> DeleteDeletePrototypeVersion(string idPrototypeVersion);

    }
}
