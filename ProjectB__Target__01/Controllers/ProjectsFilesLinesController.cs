using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectB__Target__01.Models.ProjectFileLinesModels;
using ProjectB__Target__01.Services.ProjectFilesServices;
using ProjectB__Target__01.Services.ProjectsFilesLinesServices;

namespace ProjectB__Target__01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsFilesLinesController : ControllerBase
    {
        private readonly IProjectsFileLineService _projectFileLineService;
        private readonly IProjectFileService _projectFileService;
        public ProjectsFilesLinesController(
            IProjectsFileLineService projectFileLineService,
            IProjectFileService projectFileService)
        {
            _projectFileLineService = projectFileLineService;
            _projectFileService = projectFileService;
        }

        [HttpGet("GetLinesFromContainerLinesByContainer")]
        public async Task<ActionResult> GetFileLinesFromContainerLines(string containerLines)
        {
            var res = await _projectFileLineService.GetLinesInfo(containerLines);
            return Ok(res);
        }

        [HttpPost("CreateProjectFileLines")]
        public async Task<ActionResult> CreateProjectsFilesLines(CreateProjectFileLine createProjectFileLine)
        {
            var res = await _projectFileLineService.CreateProjectFileLine(createProjectFileLine);
            if (res != null)
            {
                return Ok(res);
            }
            return BadRequest();
        }

        //[HttpPost("CreateProjectFileLinesForProjectFiles")]
        //public async Task<ActionResult> CreateProjectFileLinesForProjectFile()
        //{
        //    var files = await _projectFileService.GetAllProjectFiles();
        //    foreach (var f in files)
        //    {
        //        var lines = await _projectFileLineService.GetLinesInfo(f.IdProjectFile.ToString());
        //        if (!lines.ConainerLines.IsNullOrEmpty())
        //        {
        //            int lineNumber = 1; 
        //            foreach (var line in lines.ConainerLines)
        //            {
        //                var projectLine = new CreateProjectFileLine
        //                {
        //                    IdProjectFile = f.IdProjectFile,
        //                    Code = line,
        //                    LineNumber = lineNumber,
        //                };
        //                await _projectFileLineService.CreateProjectFileLine(projectLine);

        //                lineNumber++;
        //            }
        //        }
        //    }
        //    return Ok("lines are Created successfully");
        //}


        //using Json File and VariableType 
        [HttpGet("GetFileLinesFromContainerLinesUsingJsonFile")]
        public async Task<ActionResult> GetFileLinesFromContainerLinesUsingJsonFile(string containerLines)
        {
            var res = await _projectFileLineService.GetLinesInfoUsingVariableTypeJsonFile(containerLines);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProjectFileLine(string idProjectFile)
        {
            try
            {
                var res = await _projectFileLineService.DeleteProjectFileLine(idProjectFile);
                if (res != null)
                {
                    return Ok(res);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //[HttpPost("CreateProjectFileLinesForProjectFileUsingJsonFile")]
        //public async Task<ActionResult> CreateProjectFileLinesForProjectFileUsingJsonFile(string idContainer,Guid idProjectFile)
        //{

        //        var lines = await _projectFileLineService.GetLinesInfoUsingVariableTypeJsonFile(idContainer);
        //        if (!lines.ConainerLines.IsNullOrEmpty())
        //        {
        //            int lineNumber = 1;
        //            foreach (var line in lines.ConainerLines)
        //            {
        //                var projectLine = new CreateProjectFileLine
        //                {
        //                    IdProjectFile = idProjectFile,
        //                    Code = line,
        //                    LineNumber = lineNumber,
        //                };
        //                await _projectFileLineService.CreateProjectFileLine(projectLine);

        //                lineNumber++;
        //            }
        //        }
            
        //    return Ok("lines are Created successfully");
        //}

    }
}
