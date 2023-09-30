using ProjectB.Models;

namespace StageTest.Services.TitleServices
{
    public interface ITitleService
    {
        Task<string> GetVariableByLineCode(string lineCode);
        Task<List<string>> GetVariableIdsFromCode(string code);
        Task<string> GetVariableByLineCodeFromVariableType(string CodeLine);
        Task<List<VariableIsVerticalModel>> GetVariableIsVerticalByLineCodeFromVariableType(string CodeLine);
        Task<List<string>> GetVariableByLineCodeFromVariableTypeWithverification(string CodeLine);

        Task<List<string>> GetLines(string CodeLine);
        Task<string> GetLongestJsonDataId(List<string> idlist);
    }
}
