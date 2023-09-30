using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Services.ProjectsFoldersServices;

namespace ProjectB__Target__01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsFoldersController : ControllerBase
    {
        private readonly IProjectFolderService _projectFolderService;

        public ProjectsFoldersController(IProjectFolderService projectFolderService)
        {
            _projectFolderService = projectFolderService;
        }

        [HttpGet]
        public async Task<ActionResult> GetContainersFolders()
        {
            var res = await _projectFolderService.GetContainersFolders();
            if (res == null)
                return BadRequest();
            return Ok(res);
        }

        [HttpGet("GetFolderTitle")]
        public async Task<ActionResult> GetFolderTitle(string id)
        {
            var title = await _projectFolderService.GetFolderTitle(id);
            if(title == null)
                return BadRequest();
            return Ok(title);
        }

        [HttpGet("GetFolderDynamicTitle")]
        public async Task<ActionResult> GetFolderDynamicTitle(string dynamicTitle)
        {
            var title = await _projectFolderService.GetAllDynamicTitle(dynamicTitle);
            if (title == null)
                return BadRequest();
            return Ok(title);
        }


        [HttpPost]
        public async Task<ActionResult> CreateProjectFolder(CreateProjectsFolder projectsFolder)
        {
            var res = await _projectFolderService.CreateProjecFolder(projectsFolder);
            if(res == null)
                return BadRequest();
            return Ok(res);
        }


        [HttpPost("CreateProjectFolderFromContainerFolder")]
        public async Task<ActionResult> CreateProjectFolderFromContainerFolder()
        {
            try
            {
                var containerFolders = await _projectFolderService.GetContainersFolders();
                if (containerFolders == null || containerFolders.Count == 0)
                {
                    return BadRequest("No container folders found.");
                }

                foreach (var item in containerFolders)
                {
                    var folderTitle = await _projectFolderService.GetFolderTitle(item.IdContainerFolder);

                    var folder = new CreateProjectsFolder
                    {
                        IdProjectFolder = Guid.Parse(item.IdContainerFolder),
                        Title = folderTitle ?? ""
                    };
                    if (item.IdParent != null)
                        folder.IdParent = Guid.Parse(item.IdParent);

                    await _projectFolderService.CreateProjecFolder(folder);

                }

                return Ok("Projects Folders have been created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("ContainerFoldersByIdParent/{idParent}")]
        public async Task<ActionResult> GetContaienrFoldersByIdParent(string idParent)
        {
            var res = await _projectFolderService.GetContainersFoldersByParentId(idParent);
            if(res == null)
                return BadRequest();
            return Ok(res);
        }


        [HttpPost("ProjectFolderFromContainerFolderUsingVariableType")]
        public async Task<ActionResult> CreateProjectFolderFromContainerFolderUsingVariableType()
        {
            try
            {
                List<FolderInformations> containerFolders = await _projectFolderService.GetAllParentContainerFolder();
                if (containerFolders == null || containerFolders.Count == 0)
                {
                    return BadRequest("No container folders found.");
                }

                foreach (var item in containerFolders)
                {
                        await _projectFolderService.CreateFoldersWithIdParent(item,null,null);


                }

                return Ok("Projects Folders have been created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteFolders (string idProjectFolder)
        {
            var res = await _projectFolderService.DeleteFolder(idProjectFolder);
            if(res == null)
                return NotFound();
            return Ok(res);
        }

    }
}
