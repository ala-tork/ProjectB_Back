using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectB__Target__01.Models;
using ProjectB__Target__01.Models.ProjectsFileModels;
using System.Globalization;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;

namespace ProjectB__Target__01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //private readonly Data _data;
        // to communicate with other api
        private readonly HttpClient _httpClient;
        // to store data in a temporary memory
        private readonly IMemoryCache _cache;
        public TestController(IMemoryCache cache) { 
            _httpClient = new HttpClient();
            _cache = cache;



            // get data from json file and convert it 

            //string jsonFilePath = "test.json";

            //// Read the JSON file into a string.
            //string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

            //// Deserialize the JSON into your data structure.
            //_data = JsonConvert.DeserializeObject<Data>(jsonContent);
        }


        [HttpGet]
        public async Task<IActionResult> TestRequest()
        {
            try
            {
                string apiUrl = "https://localhost:7047/api/Conainer";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // convert data to json using a model i create
                    List<ContainerInfo> containerInfoList = JsonConvert.DeserializeObject<List<ContainerInfo>>(responseBody);
                    // store data in memory
                    _cache.Set("data", containerInfoList, TimeSpan.FromMinutes(5));

                    var res = containerInfoList.Select(info => new
                    {
                        info.idContainer,
                        info.idContainerFolder,
                        info.title
                    });

                    return Ok(res);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Api request failed");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Server Error: {e.Message}");
            }
        }

        [HttpGet("FromCache")]
        public async Task<IActionResult> GetFromCache()
        {
            if (_cache.TryGetValue("data", out List<ContainerInfo> cachedData))
            {
                // Get Data From Cache 
                var data = _cache.Get<List<ContainerInfo>>("data");
                Console.WriteLine(data.FirstOrDefault(d=>d.title=="1").idContainer);
                // show the data i got from cache
                var res = data.Select(info => new
                {
                    info.idContainer,
                    info.idContainerFolder,
                    info.title
                });
                return Ok(res);

            }
            return BadRequest("ther is no cache");

        }

        //[HttpGet("{level}")]
        //public IActionResult GetValues(char level)
        //{
        //    switch (char.ToUpper(level))
        //    {
        //        case 'A':
        //            return Ok(_data.A.Keys);
        //        case 'B':
        //            return Ok(_data.A.Values.SelectMany(levelA => levelA.B.Keys));
        //        case 'C':
        //            return Ok(_data.A.Values.SelectMany(levelA => levelA.B.Values.SelectMany(levelB => levelB.C)));
        //        default:
        //            return BadRequest("Invalid input.");
        //    }
        //}




        //[HttpGet("{level}")]
        //public IActionResult GetValues(char level)
        //{
        //    switch (char.ToUpper(level))
        //    {
        //        case 'A':
        //            return Ok(_data.A.Keys);
        //        case 'B':
        //            return Ok(_data.A.Values.SelectMany(levelA => levelA.B.Keys));
        //        case 'C':
        //            var cValues = _data.A.Values.SelectMany(levelA => levelA.B.Values.SelectMany(levelB => levelB.C.Keys));
        //            return Ok(cValues);
        //        case 'D':
        //            var dValues = _data.A.Values.SelectMany(levelA => levelA.B.Values.SelectMany(levelB=>levelB.C.Values.SelectMany(levelc=>levelc.D)));
        //            return Ok(dValues);
        //        default:
        //            return BadRequest("Invalid input.");
        //    }
        //}

    }
}
