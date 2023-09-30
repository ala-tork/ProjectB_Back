using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectB__Target__01.Models;
using ProjectB__Target__01.Models.ProjectFileLinesModels;

namespace ProjectB__Target__01.Services.ProjectsFilesLinesServices
{
    public class ProjectsFileLineService : IProjectsFileLineService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        private readonly string getContainerLinesURl = "https://localhost:7047/api/Conainer/AllLinesByContainer/";
        private readonly string GetContainerLinesUsingVariableType = "https://localhost:7047/api/Conainer/AllContainersLinesFromJson/";
        private readonly string GetContainersLinesWitheIsVerticalVerification = "https://localhost:7047/api/Conainer/GetContainersLinesFromJson/";
        public ProjectsFileLineService(ApplicationDbContext context)
        {
            _httpClient = new HttpClient();
            _context = context;
        }

        public async Task<ProjectsFilesLine> CreateProjectFileLine(CreateProjectFileLine projectsFileLine)
        {
            var fileLine = new ProjectsFilesLine
            {
                IdProjectFile = projectsFileLine.IdProjectFile,
                LineNumber = projectsFileLine.LineNumber,
                Code = projectsFileLine.Code,
            };
            await _context.ProjectsFilesLines.AddAsync(fileLine);
            await _context.SaveChangesAsync();
            return fileLine;
        }

        public async Task<List<ProjectsFilesLine>> GetAllProjectsFiles()
        {
            var res = await _context.ProjectsFilesLines.ToListAsync();
            return res;
        }

        public async Task<LinesInfo> GetLinesInfo(string projectContainerId)
        {
            var res = await  _httpClient.GetAsync(getContainerLinesURl+projectContainerId);
            if (res.IsSuccessStatusCode)
            {
                var responsBody = await res.Content.ReadAsStringAsync();
                LinesInfo lines = JsonConvert.DeserializeObject<LinesInfo>(responsBody);
                return lines;
            }
            return null;

            
        }

        public async Task<ProjectsFilesLine> GetProjectFileLineById(string id)
        {
            var res = await _context.ProjectsFilesLines.FirstOrDefaultAsync(l=>l.IdProjectFileLine == Guid.Parse(id));
            return res;
        }




        // Using Json file and variable_Types

        public async Task<LinesInfo> GetLinesInfoUsingVariableTypeJsonFile(string projectContainerId)
        {
            var res = await _httpClient.GetAsync(GetContainerLinesUsingVariableType + projectContainerId);
            if (res.IsSuccessStatusCode)
            {
                var responsBody = await res.Content.ReadAsStringAsync();
                LinesInfo lines = JsonConvert.DeserializeObject<LinesInfo>(responsBody);
                return lines;
            }
            return null;


        }

        public async Task<List<ProjectsFilesLine>> CreateProjectFileLinesForProjectFileUsingJsonFile(string idContainer, Guid idProjectFile)
        {
            var lines = await GetLinesInfoUsingVariableTypeJsonFile(idContainer);
            List<ProjectsFilesLine> res = new List<ProjectsFilesLine>();
            if (lines!=null && lines.ConainerLines.Any())
            {
                int lineNumber = 1;
                foreach (var line in lines.ConainerLines)
                {
                    var projectLine = new CreateProjectFileLine
                    {
                        IdProjectFile = idProjectFile,
                        Code = line,
                        LineNumber = lineNumber,
                    };
                    res.Add(await CreateProjectFileLine(projectLine));

                    lineNumber++;
                }
                return res;
            }
            return res;
        }


        // Using Json file and variable_Types and isvertical verification

        public async Task<LinesInfo> GetLinesInfoWithIsverticalVerification(string projectContainerId)
        {
            var res = await _httpClient.GetAsync(GetContainersLinesWitheIsVerticalVerification + projectContainerId);
            if (res.IsSuccessStatusCode)
            {
                var responsBody = await res.Content.ReadAsStringAsync();
                LinesInfo lines = JsonConvert.DeserializeObject<LinesInfo>(responsBody);
                return lines;
            }
            return null;


        }

        public async Task<List<ProjectsFilesLine>> CreateProjectFileLinesForProjectFileUsingisverticalVerification(string idContainer, Guid idProjectFile)
        {
            var lines = await GetLinesInfoWithIsverticalVerification(idContainer);
            List<ProjectsFilesLine> res = new List<ProjectsFilesLine>();
            if (lines.ConainerLines.Any())
            {
                int lineNumber = 1;
                foreach (var line in lines.ConainerLines)
                {
                    var projectLine = new CreateProjectFileLine
                    {
                        IdProjectFile = idProjectFile,
                        Code = line,
                        LineNumber = lineNumber,
                    };
                    res.Add(await CreateProjectFileLine(projectLine));

                    lineNumber++;
                }
                return res;
            }
            return res;
        }

        public async Task<ProjectsFilesLine> DeleteProjectFileLine(string idProjectFileLine)
        {
            try
            {
                var fileline = await _context.ProjectsFilesLines
                    .FirstOrDefaultAsync(fl => fl.IdProjectFileLine == Guid.Parse(idProjectFileLine));
                if(fileline != null)
                {
                    _context.ProjectsFilesLines.Remove(fileline);
                    await _context.SaveChangesAsync();
                    return fileline;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
