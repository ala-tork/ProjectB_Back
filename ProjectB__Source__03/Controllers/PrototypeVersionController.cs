using Microsoft.AspNetCore.Mvc;
using ProjectB.Models.PrototypeVersionModels;
using ProjectB.Services.PrototypeVersionServices;

namespace ProjectB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrototypeVersionController : ControllerBase
    {
        private readonly IPrototypeVersionService _prototypeVersionService;

        public PrototypeVersionController(IPrototypeVersionService prototypeVersionService)
        {
            _prototypeVersionService = prototypeVersionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrototypeVersion([FromBody] CreatePrototypeVersion createPrototypeVersion)
        {
            try
            {
                var prototypeVersion = await _prototypeVersionService.CreatePrototypeVersion(createPrototypeVersion);
                return Ok(prototypeVersion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPrototypeVersions()
        {
            try
            {
                var prototypeVersions = await _prototypeVersionService.GetAllPrototypeVersions();
                return Ok(prototypeVersions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrototypeVersionById(string id)
        {
            try
            {
                var prototypeVersion = await _prototypeVersionService.GetPrototypeVersionById(id);
                if (prototypeVersion == null)
                    return NotFound();

                return Ok(prototypeVersion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("containersfolders/{idPrototypeVersion}")]
        public async Task<IActionResult> GetContainersFoldersByIdPrototypeVersion(string idPrototypeVersion)
        {
            try
            {
                var containersFolders = await _prototypeVersionService.GetAllContainersFoldersByIdProtorypeVersion(idPrototypeVersion);
                return Ok(containersFolders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("containersfoldersParent/{idPrototypeVersion}")]
        public async Task<IActionResult> GetContainersFoldersParentByIdPrototypeVersion(string idPrototypeVersion)
        {
            try
            {
                var containersFolders = await _prototypeVersionService.GetAllContainersFoldersParentByIdProtorypeVersion(idPrototypeVersion);
                return Ok(containersFolders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<ActionResult> DeletePrototypeVersion(string idPrototypeVersion)
        {
            var res = await _prototypeVersionService.DeleteDeletePrototypeVersion(idPrototypeVersion);
            if(res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
    }
}
