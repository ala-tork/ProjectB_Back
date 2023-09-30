using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models.PrototypeVersionModels;
using StageTest.Models;
using StageTest.Models.FolderModels;
using StageTest.Services.FolderServices;

namespace ProjectB.Services.PrototypeVersionServices
{
    public class PrototypeVersionService : IPrototypeVersionService
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly IMapper _mapper;
        private readonly IFolderService _folderService;
        public PrototypeVersionService(ApplicationDbContext context, IMapper mapper , IFolderService folderService)
        {
            _DbContext = context;
            _mapper = mapper;
            _folderService = folderService;
        }
        public async Task<PrototypesVersion> CreatePrototypeVersion(CreatePrototypeVersion createPrototypeVersion)
        {
            try
            {
                var prototypeversion = _mapper.Map<PrototypesVersion>(createPrototypeVersion);
                await _DbContext.AddAsync(prototypeversion);
                await _DbContext.SaveChangesAsync();
                return prototypeversion;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PrototypesVersion> DeleteDeletePrototypeVersion(string idPrototypeVersion)
        {
            try
            {
                var prototypeVersion = await GetPrototypeVersionById(idPrototypeVersion);
                if( prototypeVersion != null )
                {
                    var prototypeVersionFolders = await _DbContext.ContainersFolders
                        .Where(f => f.IdPrototypeVersion == Guid.Parse(idPrototypeVersion))
                        .ToListAsync();
                    foreach (var folder in prototypeVersionFolders)
                    {
                        _folderService.DeleteContainerFolderAndChildren(folder.IdContainerFolder.ToString());
                    }
                    _DbContext.PrototypesVersions.Remove(prototypeVersion);
                    await _DbContext.SaveChangesAsync();
                    return prototypeVersion;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ContainersFolder>> GetAllContainersFoldersByIdProtorypeVersion(string IdProtypeVersion)
        {
            try
            {
                var containersFolders = await _DbContext.ContainersFolders
                    .Where(cf=>cf.IdPrototypeVersion == Guid.Parse(IdProtypeVersion))
                    .ToListAsync();
                return containersFolders;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ContainersFolder>> GetAllContainersFoldersParentByIdProtorypeVersion(string IdProtypeVersion)
        {
            var containersFolders = await _DbContext.ContainersFolders
                .Where(f=>f.IdPrototypeVersion == Guid.Parse(IdProtypeVersion) && f.IdParent==null)
                .ToListAsync();
            return containersFolders;
        }

        public async Task<List<PrototypesVersion>> GetAllPrototypeVersions()
        {
            try
            {
                var prototypesVersions = await _DbContext.PrototypesVersions.ToListAsync();
                return prototypesVersions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PrototypesVersion> GetPrototypeVersionById(string IdProtypeVersion)
        {
            try
            {
                var prototypeVersion = await _DbContext.PrototypesVersions
                    .FirstOrDefaultAsync(pv=>pv.IdPrototypeVersion == Guid.Parse(IdProtypeVersion));
                return prototypeVersion;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
