using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ProjectB.Models;

namespace ProjectB.Services.DtataFomJsonServices
{
    public class DataFromJsonService : IDataFromJsonService
    {
        public async  Task<List<VariableData>> GetValuesFromJsonByKeyAndJsonFilePath(string VariableTypeTitle,string filePath)
        {
            try
            {
                string jsonContent = System.IO.File.ReadAllText(filePath);
                var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent);
                List<TitleValues> result = ConvertToParentChildStructure(jsonObject, null);
                List<VariableData> names = ResultOfGetNamesByKey(VariableTypeTitle, result);
                return names;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  List<TitleValues> ConvertToParentChildStructure(JToken token, string? parentName)
        {
            var result = new List<TitleValues>();

            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    var newData = new TitleValues
                    {
                        Name = property.Name,
                        ParentName = parentName ?? null,
                    };
                    if (property.Value != null)
                        newData.Children = ConvertToParentChildStructure(property.Value, property.Name);
                    result.Add(newData);
                }
            }
            return result;
        }

        public List<VariableData> ResultOfGetNamesByKey(string key, List<TitleValues> nodes)
        {
            List<VariableData> dataItems = new List<VariableData>();

            foreach (var item in nodes)
            {
                if (item.Name == key)
                {
                    VariableData dataItem = new VariableData
                    {
                        ParentName = item.ParentName,
                        values = new List<string>()
                    };

                    foreach (var child in item.Children)
                    {
                        dataItem.values.Add(child.Name);
                    }

                    dataItems.Add(dataItem);
                }
                else if (item.Children != null)
                {
                    dataItems.AddRange(ResultOfGetNamesByKey(key, item.Children));
                }
            }

            return dataItems;
        }




        public async Task<List<VariableData>> GetValuesFromJsonByKey(string VariableTypeTitle)
        {
            try
            {
                string filePath = "test.json";
                string jsonContent = System.IO.File.ReadAllText(filePath);
                var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent);
                List<TitleValues> result = ConvertToParentChildStructure(jsonObject, null);
                List<VariableData> names = ResultOfGetNamesByKey(VariableTypeTitle, result);
                return names;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
