using Microsoft.AspNetCore.Mvc;
using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Models.ProjectVersionModels;
using ProjectB__Target__01.Services.ProjectVersionServices;


namespace ProjectB__Target__01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectVersionController : ControllerBase
    {
        private readonly IProjectVersionService _projectVersionService;

        public ProjectVersionController(IProjectVersionService projectVersionService)
        {
            _projectVersionService = projectVersionService;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectsVersion>> CreateProjectVersion([FromBody] CreateProjectVersion createProjectVersion)
        {
            try
            {
                var projectVersion = await _projectVersionService.CreateProjectVersion(createProjectVersion);
                return Ok(projectVersion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("projectFoldersInfo/{idProjectVersion}")]
        public async Task<ActionResult<List<FolderInformations>>> GetProjectFoldersInfo(string idProjectVersion)
        {
            try
            {
                var folderInfo = await _projectVersionService.GetProjectFoldersinfo(idProjectVersion);
                if (folderInfo != null)
                {
                    return Ok(folderInfo);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{idProjectVersionId}")]
        public async Task<ActionResult> GetProtoTypeVersionById(string idProjectVersionId)
        {
            try
            {
                var projectVersion = await _projectVersionService.GetPrototypeVersionByIdFromSource(idProjectVersionId);

                if (projectVersion != null)
                {
                    return Ok(projectVersion);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePrjectVersion (string idProjectVersion)
        {
            var res = await _projectVersionService.DeleteProjectVersion(idProjectVersion);
            if(res != null)
                return Ok(res);
            return NotFound();
        }

    }
}
