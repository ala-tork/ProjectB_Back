using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models.DTOS;
using StageTest.Models;
using StageTest.Models.ContainerFoldersModels;
using StageTest.Models.ContainerModels;
using StageTest.Models.FolderModels;
using StageTest.Services.ContainerServices;
using StageTest.Services.TitleServices;
using StageTest.Services.VariableServices;

namespace StageTest.Services.FolderServices
{
    public class FolderService : IFolderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITitleService _titleService;
        private readonly IContainerService _containerService;
        private readonly IVariableService _variableService;
        private readonly IMapper _mapper;
        public FolderService(ApplicationDbContext context,
            ITitleService titleService,
            IContainerService containerService,
            IMapper mapper,
            IVariableService variableService
            )
        {
            _context = context;
            _titleService = titleService;
            _containerService = containerService;
            _mapper = mapper;
            _variableService = variableService;
        }

        public async  Task<ContainersFolder> CreateContainerFolder(CreateFolder createFolderDTO)
        {
            var folder = _mapper.Map<ContainersFolder>(createFolderDTO);
            _context.ContainersFolders.Add(folder);
             await _context.SaveChangesAsync();

            return folder;
            
        }


        public async Task DeleteContainerFolderAndChildren(string id)
        {
            try
            {
                var folderId = Guid.Parse(id);
                var targetFolder = await _context.ContainersFolders.FirstOrDefaultAsync(f => f.IdContainerFolder == folderId);

                await DeleteChildrenContainersFolder(folderId);

                var folderVariables = await _context.ContainersVariables
                    .Where(v => v.IdContainerFolder == folderId)
                    .ToListAsync();
                //_context.ContainersVariables.RemoveRange(folderVariables);
                foreach (var item in folderVariables)
                {
                    await _variableService.DeleteVariabelwithchildren(item.IdVariable.ToString());
                }

                var containers = await _context.Containers
                    .Where(c => c.IdContainerFolder == folderId)
                    .ToListAsync();
                foreach (var container in containers)
                {
                    await _containerService.RemoveContainerAndChildren(container.IdContainer.ToString());
                }

                _context.ContainersFolders.Remove(targetFolder);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public async Task DeleteChildrenContainersFolder(Guid idParentFolder)
        {
            try
            {
                var folders = await _context.ContainersFolders
                    .Where(f => f.IdParent == idParentFolder)
                    .ToListAsync();

                foreach (var folder in folders)
                {

                    await DeleteChildrenContainersFolder(folder.IdContainerFolder);
                    

                    var containers = await _context.Containers
                        .Where(c => c.IdContainerFolder == folder.IdContainerFolder)
                        .ToListAsync();

                    foreach (var container in containers)
                    {
                        await _containerService.RemoveContainerAndChildren(container.IdContainer.ToString());
                    }

                    var folderVariables = await _context.ContainersVariables
                        .Where(v => v.IdContainerFolder == folder.IdContainerFolder)
                        .ToListAsync();
                    //_context.ContainersVariables.RemoveRange(folderVariables);
                    foreach (var item in folderVariables)
                    {
                        await _variableService.DeleteVariabelwithchildren(item.IdVariable.ToString());
                    }

                    _context.ContainersFolders.Remove(folder);
                }

                await _context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }


        public async Task<List<OutputContainersFoldersDto>> GetAllContainersFolders()
        {
           var containersFoldes = await _context.ContainersFolders.ToListAsync();
            var res =  _mapper.Map<List<OutputContainersFoldersDto>> (containersFoldes);
            return res;
        }

        public async Task<OutPutFolderContainersLines> GetAllContainersLinesByFolder(string idFolder)
        {
            var folder = await GetContainerFolder(idFolder);
            if (folder == null)
            {
                return null;
            }
            var folderTitle = await GetContainerFolderTitle(idFolder);
            
            if (Guid.TryParse(idFolder, out var id))
            {

                List<OutPutConainerLines> outPutContainerLines = new List<OutPutConainerLines>();
                var containers = await _context.Containers
                    .Where(c => c.IdContainerFolder == id)
                    .ToListAsync();

                foreach (var item in containers)
                {
                    var containerLines = await _containerService.GetAllLinesByContainer(item.IdContainer.ToString());
                    if (containerLines != null)
                    {
                        outPutContainerLines.Add(containerLines);
                    }
                }

                var res = new OutPutFolderContainersLines
                {
                    FolderTitle = folderTitle,
                    ContainersLines = outPutContainerLines
                };

                return res;
            }
            else
            {
                return null;
            }
        }


        public async Task<ContainersFolder> GetContainerFolder(string id)
        {

            if (Guid.TryParse(id, out var folderId))
            {
                var folder = await _context.ContainersFolders.FirstOrDefaultAsync(f => f.IdContainerFolder == folderId);
                Console.WriteLine(folder);    
                if (folder == null)
                {
                    return null;
                }
                return folder;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetContainerFolderTitle(string id)
        {
            var folder = await GetContainerFolder(id);
            if (folder == null)
            {
                return null;
            }
            if (folder.TitleDynamic != null)
                return await _titleService.GetVariableByLineCode(folder.TitleDynamic);
            else return folder.Title;
        }
        public async Task<string> GetContainerFolderTitleDynamic(string id)
        {
            var folder = await GetContainerFolder(id);
            if (folder == null)
            {
                return null;
            }
            return await _titleService.GetVariableByLineCodeFromVariableType(folder.TitleDynamic);
        }

        public async Task<List<OutputContainersFoldersDto>> GetAllContainersFoldersByParentId(string idParent)
        {
            var folders = await _context.ContainersFolders
                .Where(f=>f.IdParent==Guid.Parse(idParent))
                .ToListAsync();
            var res = _mapper.Map<List<OutputContainersFoldersDto>>(folders);
            return res;
        }

        public async Task<List<ContainersFolder>> GetAllContainersFoldersParent()
        {
            var res = await _context.ContainersFolders
                .Where(f=>f.IdParent==null)
                .ToListAsync();
            return res;
        }
    }
}
