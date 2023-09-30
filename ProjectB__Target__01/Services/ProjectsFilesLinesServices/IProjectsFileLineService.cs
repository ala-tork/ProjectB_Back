using Microsoft.AspNetCore.Mvc;
using ProjectB__Target__01.Models.ProjectFileLinesModels;
using ProjectB__Target__01.Models.ProjectsFileModels;

namespace ProjectB__Target__01.Services.ProjectsFilesLinesServices
{
    public interface IProjectsFileLineService
    {
        Task<ProjectsFilesLine> CreateProjectFileLine(CreateProjectFileLine projectsFileLine);
        Task<ProjectsFilesLine> GetProjectFileLineById(string id);
        Task<List<ProjectsFilesLine>> GetAllProjectsFiles();

        Task<LinesInfo> GetLinesInfo(string idContainer);

        Task<LinesInfo> GetLinesInfoUsingVariableTypeJsonFile(string projectContainerId);
        Task<List<ProjectsFilesLine>> CreateProjectFileLinesForProjectFileUsingJsonFile(string idContainer, Guid idProjectFile);

        Task<List<ProjectsFilesLine>> CreateProjectFileLinesForProjectFileUsingisverticalVerification(string idContainer, Guid idProjectFile);
        Task<ProjectsFilesLine> DeleteProjectFileLine(string idProjectFileLine);
    }
}
