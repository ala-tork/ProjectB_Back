using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectB.Models;
using ProjectB.Services.DtataFomJsonServices;

namespace ProjectB__Target__01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataFromJsonController : ControllerBase
    {
        private readonly IDataFromJsonService _dataFromJsonService;

        public DataFromJsonController(IDataFromJsonService dataFromJsonService)
        {
            _dataFromJsonService = dataFromJsonService;
        }

        [HttpGet("var/{VariableTypeTitle}")]
        public ActionResult<List<string>> GetNamesByKey(string VariableTypeTitle)
        {
            string filePath = "test.json";

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("JSON file not found");
            }

            try
            {
                string jsonContent = System.IO.File.ReadAllText(filePath);
                var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent);
                List<TitleValues> result = ConvertToParentChildStructure(jsonObject, null);
                List<VariableData> names = GetNamesByKey(VariableTypeTitle, result);
                return Ok(names);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<VariableData> GetNamesByKey(string key, List<TitleValues> nodes)
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
                    dataItems.AddRange(GetNamesByKey(key, item.Children));
                }
            }

            return dataItems;
        }


        [HttpGet("GetJsonTransfomation")]
        public ActionResult<List<string>> GetJsonTransfomation()
        {
            string filePath = "test.json";

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("JSON file not found");
            }

            try
            {
                string jsonContent = System.IO.File.ReadAllText(filePath);
                var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent);
                List<TitleValues> result = ConvertToParentChildStructure(jsonObject, null);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<TitleValues> ConvertToParentChildStructure(JToken token, string? parentName)
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


        [HttpGet("VariableFromJson/{VariableTypeTitle}")]
        public ActionResult<List<string>> GetNamesByKeyFromJson(string VariableTypeTitle)
        {
            string filePath = "test.json";

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("JSON file not found");
            }

            try
            {
                var names = _dataFromJsonService.GetValuesFromJsonByKeyAndJsonFilePath(VariableTypeTitle, filePath);
                return Ok(names);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
