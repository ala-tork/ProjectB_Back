using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectB__Target__01.Models.ProjectModels;
using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Models.ProjectVersionModels;
using ProjectB__Target__01.Services.ProjectFilesServices;
using ProjectB__Target__01.Services.ProjectServices;
using ProjectB__Target__01.Services.ProjectsFoldersServices;
using ProjectB__Target__01.Services.ProjectVersionServices;
using System.Linq;

namespace ProjectB__Target__01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateEverything : ControllerBase
    {
        private readonly IProjectFolderService _projectFolderService;
        private readonly IProjectFileService _projectFileService;
        private readonly IProjectVersionService _projectVersionService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        public CreateEverything(
            IProjectFolderService projectFolderService,
            IProjectFileService projectFileService,
            IProjectVersionService projectVersionService,
            IProjectService projectService,
            IMapper mapper)
        {
            _projectFolderService = projectFolderService;
            _projectFileService = projectFileService;
            _projectVersionService = projectVersionService;
            _projectService = projectService;
            _mapper = mapper;
        }


        //[HttpPost("CreateProjectFolderFromContainerFolder")]
        //public async Task<ActionResult> CreateEverythingFromSource()
        //{
        //    try
        //    {
        //        List<FolderInformations> containerFolders = await _projectFolderService.GetAllParentContainerFolder();
        //        if (containerFolders == null || containerFolders.Count == 0)
        //        {
        //            return BadRequest("No container folders found.");
        //        }

        //        foreach (var item in containerFolders)
        //        {
        //            await _projectFolderService.CreateFoldersAndFiles(item, null, null);
        //        }
        //        return Ok("Projects Folders have been created");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex}");
        //    }
        //}


        //[HttpPost("CreateProjectVersionWitheEverything")]
        //public async Task<ActionResult> CreateProjectVersionWitheEverything(string idPrototypeVersion)
        //{
        //    try
        //    {
        //        var prototypeVersion = await  _projectVersionService.GetPrototypeVersionByIdFromSource(idPrototypeVersion);
        //        if (prototypeVersion == null)
        //            return NotFound();
        //        else
        //        {
        //            var createProjectVersion = new CreateProjectVersion
        //            {
        //                Date = DateTime.Now,
        //                Title = prototypeVersion.Title,
        //                Version = prototypeVersion.Version,
        //                Description = prototypeVersion.Description,
        //                IsBackend = prototypeVersion.IsBackend,
        //                IsFrontend = prototypeVersion.IsFrontend,
        //                ShortId = prototypeVersion.ShortId,
        //                Path = prototypeVersion.Path,
        //                IsLastVersion = true,
        //            };

        //            var projectVersion = await _projectVersionService.CreateProjectVersion(createProjectVersion);

        //            List<FolderInformations> containerFolders = await _projectVersionService.GetProjectFoldersinfo(idPrototypeVersion);

        //            if (containerFolders == null || containerFolders.Count == 0)
        //            {
        //                return BadRequest("No container folders found.");
        //            }

        //            foreach (var item in containerFolders)
        //            {
        //                await _projectFolderService.CreateFoldersAndFilesFromProtypeVerson(item, null, null,projectVersion.IdProjectVersion);
        //            }
        //            return Ok("Projects Folders have been created");

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex}");
        //    }
        //}


        [HttpPost("CreateProjectVersionFromPrototypeVersion")]
        public async Task<ActionResult> CreateProjectVersionWitheEverythingAndUserIsverticalVerification(string idPrototypeVersion)
        {
            try
            {
                var prototypeVersion = await _projectVersionService.GetPrototypeVersionByIdFromSource(idPrototypeVersion);
                if (prototypeVersion == null)
                    return NotFound();
                else
                {
                    var createProjectVersion = new CreateProjectVersion
                    {
                        Date = DateTime.Now,
                        Title = prototypeVersion.Title,
                        Version = prototypeVersion.Version,
                        Description = prototypeVersion.Description,
                        IsBackend = prototypeVersion.IsBackend,
                        IsFrontend = prototypeVersion.IsFrontend,
                        ShortId = prototypeVersion.ShortId,
                        Path = prototypeVersion.Path,
                        IsLastVersion = true,
                    };

                    var projectVersion = await _projectVersionService.CreateProjectVersion(createProjectVersion);

                    List<FolderInformations> containerFolders = await _projectVersionService.GetProjectFoldersinfo(idPrototypeVersion);

                    if (containerFolders == null || containerFolders.Count == 0)
                    {
                        return BadRequest("No container folders found.");
                    }

                    foreach (var item in containerFolders)
                    {
                        await _projectFolderService.CreateFoldersAndFilesAndFileLinesFromProtypeVerson(item, null, null, projectVersion.IdProjectVersion);
                    }
                    return Ok("Projects Folders have been created");

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }

        [HttpPost("createAllFromProject")]
        public async Task<ActionResult> createAllFromProject(string idPrototype)
        {
            try
            {
                //Console.WriteLine(DateTime.Now.ToString("hh:mm:ss tt"));
                
                Project project=null;

                var existenProject = await _projectService.GetProjectById(idPrototype);
                if (existenProject == null)
                {
                     project = await _projectService.CreateProjectFromPrototype(idPrototype);
                }
                else
                {
                     project = existenProject;
                }
                

                if (project == null)
                    return NotFound();
                else
                {
                    await _projectService.UpdateoldesProjectVerions(idPrototype);

                    var prototypeVersion = await _projectService.GetPrototypeVersions(idPrototype);
                    foreach (var item in prototypeVersion)
                    {
                        var createProjectVersion = new CreateProjectVersion
                        {
                            Date = DateTime.Now,
                            IdProject= project.IdProject,
                            Title = item.Title,
                            Version = item.Version,
                            Description = item.Description,
                            IsBackend = item.IsBackend,
                            IsFrontend = item.IsFrontend,
                            ShortId = item.ShortId,
                            Path = item.Path,
                            Time = DateTime.Now.ToString("hh:mm:ss"),
                            IsLastVersion = true,
                        };

                        var projectVersion = await _projectVersionService.CreateProjectVersion(createProjectVersion);

                        List<FolderInformations> containerFolders = await _projectVersionService.GetProjectFoldersinfo(item.IdPrototypeVersion.ToString());

                        if (containerFolders == null || containerFolders.Count == 0)
                        {
                            return BadRequest("No container folders found.");
                        }

                        foreach (var folder in containerFolders)
                        {
                            await _projectFolderService.CreateFoldersAndFilesAndFileLinesFromProtypeVerson(folder, null, null, projectVersion.IdProjectVersion);
                        }
                        return Ok("Projects Folders have been created");
                    }
                }
                return Ok(project);



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }
}
