using ProjectB.Models.PrototypeModels;
using ProjectB.Models.PrototypeVersionModels;

namespace ProjectB.Services.PrototypeServices
{
    public interface IPrototypeService
    {
        Task<Prototype> CreatePrototype(CreatePrototype createPrototype);
        Task<List<Prototype>> GetAllPrototype();
        Task<Prototype> GetPrototypeById(string IdProtype);
        Task<List<PrototypesVersion>> GetAllProtorypeVersionByIdProtorype(string IdProtype);
    }
}
