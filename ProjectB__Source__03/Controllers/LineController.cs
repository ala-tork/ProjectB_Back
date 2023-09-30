using Microsoft.AspNetCore.Mvc;
using ProjectB.Models.DTOS;
using StageTest.Models.ContainerLineFolder;
using StageTest.Services.LineServices;

namespace StageTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly ILineService _lineService;
        public LineController(ILineService lineService)
        {
                _lineService = lineService;
        }

        // POST: api/CreateLine
        [HttpPost]
        public async Task<ActionResult<ContainersLine>> CreateLine([FromBody] CreateLine lineDTO)
        {
            if (lineDTO == null)
            {
                return BadRequest();
            }
            var res = await _lineService.CreateLine(lineDTO);
            if (res == null)
            {
                return BadRequest();
            }
            return Ok(res);
        }

        //Get:api/GetAllLines
        [HttpGet]
        public async Task<ActionResult> GetAllLines()
        {
            var res = await _lineService.GetAllLines();
            if (res == null)
            {
                return BadRequest();
            }
            return Ok(res);
        }

        // GET : api/GetLineById
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainersLine>> GetLineById(string id)
        {
            var res = await _lineService.GetLineById(id);
            if(res == null)
            {
                return NotFound("ther is no line with this id");
            }
            return Ok(res);
        }

        [HttpGet("GetLinesByCode/{code}")]
        public async Task<ActionResult> GetLinesByCode(string code)
        {
            var res = await _lineService.GetLinesIds(code);
            if (res == null)
                return NotFound("Ther is no Line with this id");
            return Ok(res);
        }

        [HttpGet("GetTitle/{lineId}")]
        public async Task<ActionResult> GetTitle(string lineId)
        {
            var res = await _lineService.LinesTitles(lineId);
            if (res == null)
                return NotFound("Ther is no Line with this id");
            return Ok(res);
        }


        [HttpGet("GetLineTitleFromJson/{lineId}")]
        public async Task<ActionResult> GetLineTitleFromJson(string lineId)
        {
            var res = await _lineService.LinesTitlesFromVariabeType(lineId);
            if (res == null)
                return NotFound("Ther is no Line with this id");
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteContainerLine(string id)
        {
            var res = await _lineService.DeleteLine(id);
            if (res == null)
                return NotFound();
            return Ok(res);
        }

        ////isvertical
        //[HttpGet("GetLineTitleHorizontalOrVertical/{lineId}")]
        //public async Task<ActionResult> GetLineTitleHorizontalOrVertical(string lineId)
        //{
        //    var res = await _lineService.LinesTitlesFromVariabeTypeVerticalOrHorizontal(lineId);
        //    if (res == null)
        //        return NotFound("Ther is no Line with this id");
        //    return Ok(res);
        //}


        //Using isvertical in ContainerLine for verification
        [HttpGet("GetLineContent/{lineId}")]
        public async Task<ActionResult> GetLineContent(string lineId)
        {
            var res = await _lineService.LinesTitlesFromVariabeTypeUsingIsVerticalAlign(lineId);
            if (res == null)
                return NotFound("Ther is no Line with this id");
            return Ok(res);
        }


    }
}
