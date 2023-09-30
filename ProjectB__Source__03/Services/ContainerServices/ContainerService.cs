using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models.DTOS;
using StageTest.Models;
using StageTest.Models.ContainerModels;
using StageTest.Services.LineServices;
using StageTest.Services.TitleServices;
using StageTest.Services.VariableServices;

namespace StageTest.Services.ContainerServices
{
    public class ContainerService:IContainerService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ITitleService _titleService;
        private readonly ILineService _linesService;
        private readonly IVariableService _variableService;

        public ContainerService(
            ApplicationDbContext context,
            ITitleService titleService, 
            ILineService lineService,
            IVariableService variableService,
            IMapper mapper
            )
        {
            _context = context;
            _titleService = titleService;
            _linesService = lineService;
            _mapper = mapper;
            _variableService = variableService;
        }

        public async Task<OutPutConainerLines> GetAllLinesByContainer(string idContainer)
        {
            if (Guid.TryParse(idContainer, out var id))
            {
                var container = await _context.Containers.FirstOrDefaultAsync(c => c.IdContainer == id);
                if (container == null)
                {
                    return null;
                }
                var listContainerLines = await _linesService.GetallContainerLinesTite(idContainer);
                var res = new OutPutConainerLines
                {
                    ContainerTitle = await GetContainerTitle(idContainer),
                    ConainerLines = listContainerLines 
                };
                return res;
            }
            return null;
        }


        public async Task<List<OutputContainerDto>> GetAllContainers()
        {
            //var res =await _context.Containers.OrderBy(c=>c.Title).ToListAsync();
            var containers = await _context.Containers.ToListAsync();
            var res = _mapper.Map<List<OutputContainerDto>>(containers);
            return res;
        }


        public async Task<List<Dictionary<string, string>>> GetAllfolderContainersLines(string idFolder)
        {
            if (!Guid.TryParse(idFolder, out var id))
            {
                throw new ArgumentException("Invalid Folder ID");
            }

            var containers = await _context.Containers
                .Where(c => c.IdContainerFolder == id)
                .ToListAsync();

            var titles = new List<Dictionary<string, string>>();

            foreach (var container in containers)
            {
                var containerTitle = await GetContainerTitle(container.IdContainer.ToString());
                var containerLines = await _linesService.GetallContainerLinesTite(container.IdContainer.ToString());

                var resultat = new Dictionary<string, string>
                    {
                        { "Container Title", containerTitle },
                        { "Container Lines", string.Join(", ", containerLines) }
                    };

                titles.Add(resultat);
            }

            return titles;
        }






        public async  Task<Container> GetContainerById(string id)
        {
            if(Guid.TryParse(id, out var containerId))
            {
                var res = await _context.Containers.FirstOrDefaultAsync(c => c.IdContainer == containerId);
                return res;
            }
            return null;

        }

        public async Task<string> GetContainerTitle(string containerId)
        {
              var container = await GetContainerById(containerId);
            if(container == null)
            {
                return null;
            }

            if(container.Title != null)
            {
                return await _titleService.GetVariableByLineCode(container.Title);
            }
            else
            {
                return await _titleService.GetVariableByLineCode(container.TitleDynamic);
            }
        }

        public async Task<string> GetContainerDynamicTitle(string ContainerId)
        {
            var container = await GetContainerById(ContainerId);
            if (container == null)
            {
                return null;
            }

            if (container.Title != null)
            {
                return container.Title;
            }
            else
            {
                return await _titleService.GetVariableByLineCodeFromVariableType(container.TitleDynamic);
            }
        }

        public async Task<Container> CreateContainer(CreateContainer createContainer)
        {
            try
            {
                var container = _mapper.Map<Container>(createContainer);
                await _context.Containers.AddAsync(container);
                await _context.SaveChangesAsync();
                return container;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task RemoveContainerAndChildren(string containerId)
        {
            try
            {
                var idContainer = Guid.Parse(containerId);
                var targetContainer = await _context.Containers.FirstOrDefaultAsync(c => c.IdContainer == idContainer);
                await DeleteChildrenContainers(idContainer);

                var containervariables = await _context.ContainersVariables
                    .Where(v => v.IdContainer == idContainer)
                    .ToListAsync();

                    foreach (var item in containervariables)
                    {
                        await _variableService.DeleteVariabelwithchildren(item.IdVariable.ToString());
                    }
                


                var targetContainerLines = await _context.ContainersLines
                    .Where(cl => cl.IdContainer == idContainer)
                    .ToListAsync();

                foreach (var containerLine in targetContainerLines)
                {
                    await _linesService.DeleteLine(containerLine.IdContainerLine.ToString());
                }
                _context.Containers.Remove(targetContainer);
                await _context.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        async Task DeleteChildrenContainers(Guid parentId)
        {
            try
            {
                var childrenContainers = await _context.Containers
                    .Where(c => c.IdParent == parentId)
                    .ToListAsync();

                foreach (var child in childrenContainers)
                {
                    await DeleteChildrenContainers(child.IdContainer);

                    var containerLines = await _context.ContainersLines
                        .Where(cl => cl.IdContainer == child.IdContainer)
                        .ToListAsync();

                    foreach (var containerLine in containerLines)
                    {
                        await _linesService.DeleteLine(containerLine.IdContainerLine.ToString());
                    }
                    var container_variables = await _context.ContainersVariables
                            .Where(v => v.IdContainer == child.IdContainer)
                            .ToListAsync();
                    foreach (var item in container_variables)
                    {
                        await _variableService.DeleteVariabelwithchildren(item.IdVariable.ToString());
                        
                    }
                    _context.Containers.Remove(child);
                }

            }catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<OutputContainerDto>> GetAllContainersByIdContainerFolder(string containerFolderId)
        {
            var containers = await _context.Containers
                .Where(c=>c.IdContainerFolder == Guid.Parse(containerFolderId))
                .ToListAsync();
            var res  = _mapper.Map<List<OutputContainerDto>>(containers);
            return res;
        }

        public async Task<List<OutputContainerDto>> GetContainerByIdParent(string idParent)
        {
            var res = await _context.Containers
                .Where(c=>c.IdParent == Guid.Parse(idParent))
                .ToListAsync();
            var containers = _mapper.Map<List<OutputContainerDto>>(res);
            return containers;
        }

        public async Task<OutPutConainerLines> GetAllLinesByContainerFromJsonFile(string idContainer)
        {
            if (Guid.TryParse(idContainer, out var id))
            {
                var container = await _context.Containers.FirstOrDefaultAsync(c => c.IdContainer == id);
                if (container == null)
                {
                    return null;
                }
                var listContainerLines = await _linesService.GetallContainerLinesTiteFromJson(idContainer);
                var res = new OutPutConainerLines
                {
                    ContainerTitle = await GetContainerTitle(idContainer),
                    ConainerLines = listContainerLines
                };
                return res;
            }
            return null;
        }

        //isvertical 
        public async Task<OutPutConainerLines> GetLinesByContainerFromJsonFile(string idContainer)
        {
            if (Guid.TryParse(idContainer, out var id))
            {
                var container = await _context.Containers.FirstOrDefaultAsync(c => c.IdContainer == id);
                if (container == null)
                {
                    return null;
                }
                var listContainerLines = await _linesService.GetallContainerLinesTiteFromJsonHorizontalOrVertical(idContainer);
                var res = new OutPutConainerLines
                {
                    ContainerTitle = await GetContainerTitle(idContainer),
                    ConainerLines = listContainerLines
                };
                return res;
            }
            return null;
        }
    }
}
