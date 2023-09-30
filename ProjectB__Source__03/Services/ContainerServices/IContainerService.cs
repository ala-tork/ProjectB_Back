using ProjectB.Models.DTOS;
using StageTest.Models.ContainerModels;

namespace StageTest.Services.ContainerServices
{
    public interface IContainerService
    {
        Task<Container> CreateContainer(CreateContainer container);
        Task<List<OutputContainerDto>> GetAllContainers();
        Task<List<OutputContainerDto>> GetAllContainersByIdContainerFolder(string containerFolderId);
        Task<Container> GetContainerById(string id);
        Task<List<OutputContainerDto>> GetContainerByIdParent(string idParent);
        Task<string> GetContainerTitle(string Containerid);
        Task<string> GetContainerDynamicTitle(string ContainerId);
        Task<OutPutConainerLines> GetAllLinesByContainer(string idContainer);
        Task<OutPutConainerLines> GetAllLinesByContainerFromJsonFile(string idContainer);
        Task<OutPutConainerLines> GetLinesByContainerFromJsonFile(string idContainer);
        Task RemoveContainerAndChildren(string containerId);
    }
}
