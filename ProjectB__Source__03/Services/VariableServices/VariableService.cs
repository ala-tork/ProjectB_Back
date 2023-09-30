using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models.DTOS;
using StageTest.Models;
using StageTest.Models.ContainersVariablesModels;
using System.Collections.ObjectModel;

namespace StageTest.Services.VariableServices
{
    public class VariableService : IVariableService
    {
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _mapper;
        public VariableService(
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            _Context = context;
            _mapper = mapper;
        }

        public async Task<ContainersVariable> CreateVariable(CreateVariable variable)
        {
            var Cvariable = _mapper.Map<ContainersVariable>( variable );
            await _Context.ContainersVariables.AddAsync(Cvariable);
            await _Context.SaveChangesAsync();
            return Cvariable;
        }

        public async Task DeleteVariabelwithchildren(string id)
        {
            try
            {
                var idContainerVariable = Guid.Parse(id);

                var targetVariable = await _Context.ContainersVariables
                    .FirstOrDefaultAsync(c => c.IdVariable == idContainerVariable);

                await deletechildrenvariable(idContainerVariable);

                _Context.ContainersVariables.Remove(targetVariable);

                await _Context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        async Task deletechildrenvariable(Guid id) 
        {
            try
            {
                var childrenvariables = await _Context.ContainersVariables
                    .Where(c => c.IdParent == id)
                    .ToListAsync();

                foreach (var child in childrenvariables)
                {
                    await deletechildrenvariable(child.IdVariable);
                    _Context.ContainersVariables.Remove(child);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<ContainersVariable> DeleteVariable(string id)
        //{
        //    try
        //    {
        //        var res = await _Context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == Guid.Parse(id));

        //        if (res != null)
        //        {
        //            _Context.ContainersVariables.Remove(res);
        //            await _Context.SaveChangesAsync();
        //            return res;
        //        }
        //        return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public async Task<List<ContainersVariable>> GetAllVariables()
        {
            var res = await _Context.ContainersVariables.ToListAsync();
            return res;
        }

        public async Task<ContainersVariable> GetVariableById(string id)
        {
            if(Guid.TryParse(id, out var variableId))
            {
                var res = await _Context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == variableId);
                if (res == null)
                    return null;
                return res;
            }
            return null;
           
        }

        public async Task<ContainersVariable> UpdateVariable(CreateVariable variable, string id)
        {
            ContainersVariable existingVariable = await GetVariableById(id);

            if (existingVariable != null)
            {
                _mapper.Map(variable, existingVariable);

                _Context.Update(existingVariable);
                await _Context.SaveChangesAsync();
                return existingVariable;
            }
            return null;

        }
    }
}
