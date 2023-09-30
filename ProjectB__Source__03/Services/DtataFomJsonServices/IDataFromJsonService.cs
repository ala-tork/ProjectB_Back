using Newtonsoft.Json.Linq;
using ProjectB.Models;

namespace ProjectB.Services.DtataFomJsonServices
{
    public interface IDataFromJsonService
    {
        Task<List<VariableData>> GetValuesFromJsonByKeyAndJsonFilePath(string VariableTypeTitle, string filePath);
        List<TitleValues> ConvertToParentChildStructure(JToken token, string? parentName);
        List<VariableData> ResultOfGetNamesByKey(string key, List<TitleValues> nodes);

        Task<List<VariableData>> GetValuesFromJsonByKey(string VariableTypeTitle);
    }
}
