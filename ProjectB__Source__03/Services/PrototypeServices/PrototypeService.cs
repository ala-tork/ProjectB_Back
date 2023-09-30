using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models.PrototypeModels;
using ProjectB.Models.PrototypeVersionModels;
using StageTest.Models;

namespace ProjectB.Services.PrototypeServices
{
    public class PrototypeService : IPrototypeService
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly IMapper _mapper;
        public PrototypeService(ApplicationDbContext context,IMapper mapper)
        {
            _DbContext = context;
            _mapper = mapper;
        }
        public async Task<Prototype> CreatePrototype(CreatePrototype createPrototype)
        {
            try
            {
                var prototype = _mapper.Map<Prototype>(createPrototype);
                await _DbContext.Prototypes.AddAsync(prototype);
                await _DbContext.SaveChangesAsync();
                return prototype;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<PrototypesVersion>> GetAllProtorypeVersionByIdProtorype(string IdProtype)
        {
            try
            {
                var prtptypeVersion = await _DbContext.PrototypesVersions
                    .Where(pv => pv.IdPrototype == Guid.Parse(IdProtype))
                    .ToListAsync();
                return prtptypeVersion;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Prototype>> GetAllPrototype()
        {
            try
            {
                var prototypes = await _DbContext.Prototypes.ToListAsync();
                return prototypes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Prototype> GetPrototypeById(string IdProtype)
        {
            try
            {
                var prtotype = await _DbContext.Prototypes
                    .FirstOrDefaultAsync(p => p.IdPrototype == Guid.Parse(IdProtype));
                return prtotype;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
