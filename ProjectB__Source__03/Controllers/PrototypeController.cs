using Microsoft.AspNetCore.Mvc;
using ProjectB.Models.PrototypeModels;
using ProjectB.Services.PrototypeServices;

namespace ProjectB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrototypeController : ControllerBase
    {
        private readonly IPrototypeService _prototypeService;

        public PrototypeController(IPrototypeService prototypeService)
        {
            _prototypeService = prototypeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrototype([FromBody] CreatePrototype createPrototype)
        {
            try
            {
                var prototype = await _prototypeService.CreatePrototype(createPrototype);
                return Ok(prototype);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPrototypes()
        {
            try
            {
                var prototypes = await _prototypeService.GetAllPrototype();
                return Ok(prototypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrototypeById(string id)
        {
            try
            {
                var prototype = await _prototypeService.GetPrototypeById(id);
                if (prototype == null)
                    return NotFound();

                return Ok(prototype);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PrototypesVersions/{idPrototype}")]
        public async Task<IActionResult> GetPrototypeVersions(string idPrototype)
        {
            try
            {
                var versions = await _prototypeService.GetAllProtorypeVersionByIdProtorype(idPrototype);
                return Ok(versions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

