using Microsoft.VisualBasic;
using ProjectB.Models.DTOS;
using StageTest.Models.ContainersVariablesModels;
using System.Collections.ObjectModel;

namespace StageTest.Services.VariableServices
{
    public interface IVariableService
    {
        Task<ContainersVariable> CreateVariable(CreateVariable variable);
        Task<List<ContainersVariable>> GetAllVariables();
        Task<ContainersVariable> GetVariableById(string id);
        //Task<ContainersVariable> DeleteVariable(string id);
        Task<ContainersVariable> UpdateVariable(CreateVariable variable, string id);
        Task DeleteVariabelwithchildren(string id);
    }
}
