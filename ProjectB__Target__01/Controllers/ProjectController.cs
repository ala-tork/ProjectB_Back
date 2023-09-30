using Microsoft.AspNetCore.Mvc;
using ProjectB__Target__01.Models.ProjectModels;
using ProjectB__Target__01.Services.ProjectServices;

namespace ProjectB__Target__01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProject createProject)
        {
            try
            {
                var project = await _projectService.CreateProject(createProject);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-prototype/{idPrototype}")]
        public async Task<IActionResult> GetPrototypeById(string idPrototype)
        {
            try
            {
                var prototype = await _projectService.GetPrototypeById(idPrototype);
                if (prototype == null)
                {
                    return NotFound();
                }
                return Ok(prototype);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-prototype-versions/{idProject}")]
        public async Task<IActionResult> GetPrototypeVersions(string idProject)
        {
            try
            {
                var prototypeVersions = await _projectService.GetPrototypeVersions(idProject);
                if (prototypeVersions == null)
                {
                    return NotFound();
                }
                return Ok(prototypeVersions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProject ( string idProject)
        {
            var res = await _projectService.DeleteProject(idProject);
            if (res == null)
                return NotFound();
            return Ok(res);
        }
    }
}
