using ProjectB.Models.DTOS;
using StageTest.Models.ContainerLineFolder;

namespace StageTest.Services.LineServices
{
    public interface ILineService
    {
        Task<ContainersLine> CreateLine(CreateLine lineDTO);
        Task<List<ContainersLine>> GetAllLines();
        Task<ContainersLine> GetLineById(string id);
        //Task<string> GetLinesTitle(string id);
        Task<List<string>> GetallContainerLinesTite(string idContainer);
        Task<ContainersLine> DeleteLine(string id);
        Task<List<ContainersLine>> GetLinesIds(string code);

        Task<List<string>> LinesTitles(string id);
        Task<List<string>> LinesTitlesFromVariabeType(string id);
        Task<List<string>> GetallContainerLinesTiteFromJson(string containerId);
        Task<List<string>> GetallContainerLinesTiteFromJsonHorizontalOrVertical(string containerId);
        //Task<List<string>> LinesTitlesFromVariabeTypeVerticalOrHorizontal(string id);


        Task<List<string>> LinesTitlesFromVariabeTypeUsingIsVerticalAlign(string id);
    }
}
