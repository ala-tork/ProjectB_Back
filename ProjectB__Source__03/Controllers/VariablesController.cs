using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectB.Models.DTOS;
using StageTest.Models.ContainersVariablesModels;
using StageTest.Services.VariableServices;

namespace StageTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariablesController : ControllerBase
    {
        private readonly IVariableService _variableService;

        public VariablesController(IVariableService variableService)
        {
            _variableService = variableService;
        }

        // POST : api/VariablesController
        [HttpPost]
        public async Task<IActionResult> CreateVariable(CreateVariable variableDTO)
        {
            if (variableDTO == null)
            {
                return BadRequest();
            }
            var res = await _variableService.CreateVariable(variableDTO);
            return Ok(res);
        }

        // Get:api/GetAllVariables
        [HttpGet]
        public async Task<ActionResult<List<ContainersVariable>>> GetAllVariables()
        {
            var res = await _variableService.GetAllVariables();
            return Ok(res);
        }

        // Get:api/GetVariable/idVariabe
        [HttpGet("{variableId}")]
        public async Task<ActionResult<ContainersVariable>> GetVariable(string variableId)
        {
            var res = await _variableService.GetVariableById(variableId);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainerVariable(string id)
        {
            var variable = await _variableService.GetVariableById(id);

            if (variable == null)
            {
                return NotFound();
            }

            await _variableService.DeleteVariabelwithchildren(id);


            return Ok("Variable deleted successfully");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVariables(CreateVariable variableDTO,string id)
        {
            if(variableDTO == null)
                return BadRequest();
            var res = await _variableService.UpdateVariable(variableDTO, id);
            return Ok(res);
        }
    }
}
