using Microsoft.AspNetCore.Mvc;
using ProjectB.Models.DTOS;
using StageTest.Models.ContainerModels;
using StageTest.Models.FolderModels;
using StageTest.Services.FolderServices;

namespace StageTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _foldeService;
        public FolderController(IFolderService foldeService)
        {
            _foldeService = foldeService;
        }

        // POST: api/ContainersFolder
        [HttpPost]
        public async Task<ActionResult<ContainersFolder>> CreateFolder([FromBody] CreateFolder createFolderDTO)
        {
            if(createFolderDTO == null)
            {
                return BadRequest();
            }
            var res = await  _foldeService.CreateContainerFolder(createFolderDTO);

            return Ok(res);  
        }

        // get: api/GetAllFlders
        [HttpGet]
        public async Task<IActionResult> GetAllFlders()
        {
            var folders = await _foldeService.GetAllContainersFolders();
            return Ok(folders);
        }

        // GEt: api/ContainersFolder/{folderID}
        [HttpGet("/{folderID}")]
        public async Task<IActionResult> GetFlder (string folderID)
        {
            var folder = await _foldeService.GetContainerFolder(folderID);
            if(folder == null)
            {
                return NotFound("Folder Not Found!");
            }
            return Ok(folder);
        }

        [HttpGet("ContainerFolderByIdParent/{idFolder}")]
        public async Task<IActionResult> GetContainerFolderByIdParent(string idFolder)
        {
            var folder = await _foldeService.GetAllContainersFoldersByParentId(idFolder);
            if (folder == null)
            {
                return NotFound("Folder Not Found!");
            }
            return Ok(folder);
        }
        // GEt: api/ContainersFolderTitle/{folderId}
        [HttpGet("FolderTitle/{folderId}")]
        public async Task<ActionResult> GetFolderTitle(string folderId)
        {
            var res = await _foldeService.GetContainerFolderTitle(folderId);
            if (res == null)
                return NotFound("ther is no folder with this id");
            return Ok(res);
        }

        [HttpGet("FolderTitleDynamic/{folderId}")]
        public async Task<ActionResult> GetFolderTitleDynamic(string folderId)
        {
            var res = await _foldeService.GetContainerFolderTitleDynamic(folderId);
            if (res == null)
                return NotFound("ther is no folder with this id");
            return Ok(res);
        }

        // Get : api/AllFolderContainersLines/idFolder
        [HttpGet("AllContainers&&LinesByFolder/idFolder")]
        public async Task<ActionResult> GetAllFolderContainersLines(string idFolder)
        {
            var res = await _foldeService.GetAllContainersLinesByFolder(idFolder);
            if (res == null)
                return NotFound("Ther Is no Folder With this id");
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainersFolders(string id)
        {
            var container = await _foldeService.GetContainerFolder(id);

            if (container == null)
            {
                return NotFound();
            }

            await _foldeService.DeleteContainerFolderAndChildren(id);


            return Ok("ContainersFolders deleted successfully");
        }

        [HttpGet("AllParentContainersFolders")]
        public async Task<ActionResult> GetAllParentContainerFolder()
        {
            var res = await _foldeService.GetAllContainersFoldersParent();
            return Ok(res);
        }
    }
}
