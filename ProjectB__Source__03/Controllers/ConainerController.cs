using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models.DTOS;
using StageTest.Services.ContainerServices;
using System.Runtime.InteropServices;

namespace StageTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConainerController : ControllerBase
    {
        private readonly IContainerService _containerService;
        public ConainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }
        [HttpPost]
        public async Task<ActionResult> CreateContainer(CreateContainer createContainer)
        {
            if (createContainer == null)
            {
                return BadRequest();
            }
            var res = await _containerService.CreateContainer(createContainer);
            return Ok(res);
        }

        //Get : api/GetAllContainers
        [HttpGet]
        public async Task<ActionResult> GetAllContainers()
        {
            var res = await _containerService.GetAllContainers();
            return Ok(res);
        }
        //GEt : api/GetContainreById/containerId
        [HttpGet("{containerId}")]
        public async Task<ActionResult> GetContainreById(string containerId)
        {
            var res = await _containerService.GetContainerById(containerId);
            if (res == null)
                return NotFound("ther is no container with this is");
            return Ok(res);
        }

        //GEt : api/GetAllContainresByIdContainerFolder/containerFolderId
        [HttpGet("ContainersByContainerFolder/{containerFolderId}")]
        public async Task<ActionResult> GetAllContainresByIdContainerFolder(string containerFolderId)
        {
            var res = await _containerService.GetAllContainersByIdContainerFolder(containerFolderId);
            if (res == null)
                return NotFound("ther is no container Folder with this is");
            return Ok(res);
        }

        //Get : api/GetConainerTitle/containerId
        [HttpGet("ContainreTitle/{containerId}")]
        public async Task<ActionResult> GetContainerTitle(string containerId)
        {
            var res = await _containerService.GetContainerTitle(containerId);
            if (res == null)
                return NotFound("ther is no container with this id");
            return Ok(res);
        }
        //Get : api/GetConainerDynamicTitle/containerId
        [HttpGet("ContainreDynamicTitle/{containerId}")]
        public async Task<ActionResult> GetContainerDynamicTitle(string containerId)
        {
            var res = await _containerService.GetContainerDynamicTitle(containerId);
            if (res == null)
                return NotFound("ther is no container with this id");
            return Ok(res);
        }

        //Get : api/AllContainersLines/idFolder
        [HttpGet("AllLinesByContainer/{idContainer}")]
        public async Task<ActionResult> AllLinesByContainer(string idContainer)
        {
            var res = await _containerService.GetAllLinesByContainer(idContainer);
            if (res == null)
                return NotFound("Ther is no Container with this id !");
            return Ok(res);
        }

        //GEt : api/GetContainresByIdParent/containerFolderId
        [HttpGet("ContainersByIdParent/{IdParent}")]
        public async Task<ActionResult> GetContainresByIdParent(string IdParent)
        {
            var res = await _containerService.GetContainerByIdParent(IdParent);
            if (res == null)
                return NotFound($"ther is no container  with this is ${IdParent}");
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainer(string id)
        {
            var container = await _containerService.GetContainerById(id);

            if (container == null)
            {
                return NotFound(); 
            }

            await _containerService.RemoveContainerAndChildren(id);
           

            return Ok("Container deleted successfully"); 
        }



        ////Get : api/AllContainersLinesFromJson/idFolder
        //[HttpGet("AllContainersLinesFromJson/{idContainer}")]
        //public async Task<ActionResult> GetAllContainersLinesFromJson(string idContainer)
        //{
        //    var res = await _containerService.GetAllLinesByContainerFromJsonFile(idContainer);
        //    if (res == null)
        //        return NotFound("Ther is no Container with this id !");
        //    return Ok(res);
        //}


        //isvertical
        //Get : api/AllContainersLinesFromJson/idFolder
        [HttpGet("GetContainersLinesFromJson/{idContainer}")]
        public async Task<ActionResult> GetContainersLinesFromJson(string idContainer)
        {
            var res = await _containerService.GetLinesByContainerFromJsonFile(idContainer);
            if (res == null)
                return NotFound("Ther is no Container with this id !");
            return Ok(res);
        }
    }
}
