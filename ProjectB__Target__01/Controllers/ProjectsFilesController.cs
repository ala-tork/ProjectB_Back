using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ProjectB__Target__01.Models.ProjectsFileModels;
using ProjectB__Target__01.Services.ProjectFilesServices;

namespace ProjectB__Target__01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsFilesController : ControllerBase
    {
        private readonly IProjectFileService _projectFileService;

        public ProjectsFilesController(IProjectFileService projectFileService)
        {
            _projectFileService = projectFileService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProjectFile(CreateProjectsFile createProjectsFile)
        {
            if (createProjectsFile == null)
            {
                return BadRequest();
            }
            var res = await _projectFileService.CreateProjectFile(createProjectsFile);
            return Ok(res);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllFiles()
        {
            var res =await  _projectFileService.GetAllProjectFiles();
            return Ok(res);
        }


        [HttpGet("GetContainersInfo")]
        public async Task<ActionResult> GetContainers()
        {
            var res  = await _projectFileService.GetContainers();
            if (res == null)
                return BadRequest();
            return Ok(res);
        }

        [HttpGet("ContaienrsByIdContainerFolder/{idContainerFolder}")]
        public async Task<ActionResult> GetAllContaienrsByFolderId(string idContainerFolder)
        {
            var res  = await _projectFileService.GetAllContainersByIdContainerFolder(idContainerFolder);
            if (res == null)
                return NotFound("thers no containers With this Id Contaiern Folder");
            return Ok(res);
        }
        [HttpGet("ContaienrsByIdParent/{IdParent}")]
        public async Task<ActionResult> GetContaienrsByIdParent(string IdParent)
        {
            var res = await _projectFileService.GetContainersByIdParent(IdParent);
            if (res == null)
                return NotFound($"thers no containers With this Id ${IdParent}");
            return Ok(res);
        }

        [HttpPost("createProjectFilesFromContainers")]
        public async Task<ActionResult> createProjectFilesFromContainers()
        {
            var containers = await _projectFileService.GetContainers();
            if(containers == null)
            {
                return NotFound("THer is no containers check Containers Server");
            }
            foreach (var item in containers)
            {       
                    
                    var fileTitle = await _projectFileService.GetContainerTitle(item.idContainer);

                    var file = new CreateProjectsFile
                    {
                        IdProjectFile = Guid.Parse(item.idContainer),
                        Title = fileTitle ?? "",
                    };

                    if (item.idContainerFolder != null)
                    {
                        file.IdProjectFolder = Guid.Parse(item.idContainerFolder);
                    }
                    await _projectFileService.CreateProjectFile(file);


            }
            return Ok("Project Files Created successfully ");
        }

        [HttpGet("DynamicContainersTitles/{IdContainer}")]
        public async Task<ActionResult> GetDynamicTitle(string IdContainer)
        {
            var res = await _projectFileService.GetAllContainersDynamicTitle(IdContainer);
            return Ok(res);
        }
        [HttpGet("DynamicContainersTitlesFromJson/{IdContainer}")]
        public async Task<ActionResult> GetDynamicTitleFromJson(string IdContainer)
        {
            var res = await _projectFileService.GetAllContainersDynamicTitleFromJson(IdContainer);
            return Ok(res);
        }

        //[HttpPost("createProjectFilesFromContainersWithDynamicTitle")]
        //public async Task<ActionResult> createProjectFilesFromContainersWithDynamicTitle()
        //{
        //    var containers = await _projectFileService.GetContainers();
        //    if (containers == null)
        //    {
        //        return NotFound("THer is no containers check Containers Server");
        //    }
        //    foreach (var item in containers)
        //    {
        //        if(item.title != null)
        //        {
        //            var file = new CreateProjectsFile
        //            {
        //                IdProjectFile = Guid.Parse(item.idContainer),
        //                Title = item.title ?? "",
        //            };

        //            if (item.idContainerFolder != null)
        //            {
        //                file.IdProjectFolder = Guid.Parse(item.idContainerFolder);
        //            }
        //            await _projectFileService.CreateProjectFile(file);
        //        }
        //        else if(item.TitleDynamic != null)
        //        {
        //            var filesTitles = await _projectFileService.GetAllContainersDynamicTitle(item.idContainerFolder);
        //            foreach (var titles in filesTitles)
        //            {
        //                foreach (var title in titles.values)
        //                {
        //                    var file = new CreateProjectsFile
        //                    {
        //                        IdProjectFile = Guid.Parse(item.idContainer),
        //                        Title = title,
        //                    };

        //                    if (item.idContainerFolder != null)
        //                    {
        //                        file.IdProjectFolder = Guid.Parse(item.idContainerFolder);
        //                    }
        //                    await _projectFileService.CreateProjectFile(file);

        //                }
        //            }
        //        }




        //    }
        //    return Ok("Project Files Created successfully ");
        //}


        [HttpPost("createFilesByContainerFolderId/{idContainerFolder}/{idFolder}")]
        public async Task<ActionResult> create (string idContainerFolder, Guid idFolder)
        {
            var res = await _projectFileService.createDynamicFilesByIdContainer(idContainerFolder, idFolder);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteFile(string idProjectFile)
        {
            var res = await _projectFileService.DeleteProjectFile(idProjectFile);
            if(res!=null)
                return Ok(res);
            return NotFound();
        }
    }
}
