using Microsoft.AspNetCore.Mvc;
using StageTest.Services.TitleServices;

namespace StageTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly ITitleService _titleService;

        public TitleController(ITitleService titleService)
        {
            _titleService = titleService;
        }



        //Get : api/LineTitle/CodeLine
        [HttpGet("LineTitle/{CodeLine}")]
        public async Task<ActionResult<string>> GetTitleLine(string CodeLine)
        {
            var res = await _titleService.GetVariableByLineCode(CodeLine);
            if (res == null)
                return NotFound("Ther is no title with this id");
            return Ok(res);
        }

        //Get : api/LineTitle/CodeLine
        [HttpGet("TitleByVariableType/{CodeLine}")]
        public async Task<ActionResult<string>> GetTitleByVariableType(string CodeLine)
        {
            var res = await _titleService.GetVariableByLineCodeFromVariableType(CodeLine);
            if (res == null)
                return NotFound("Ther is no title with this id");
            return Ok(res);
        }

        //Get : api/LineTitle/CodeLine
        [HttpGet("TitleByVariableTypeAndIsVertical/{CodeLine}")]
        public async Task<ActionResult<string>> GetTitleByVariableTypeAndIsVertical(string CodeLine)
        {
            var res = await _titleService.GetVariableIsVerticalByLineCodeFromVariableType(CodeLine);
            if (res == null)
                return NotFound("Ther is no title with this id");
            return Ok(res);
        }


        //Get : api/LineTitle/CodeLine
        [HttpGet("Title/{CodeLine}")]
        public async Task<ActionResult<string>> GetTitle(string CodeLine)
        {
            var res = await _titleService.GetLines(CodeLine);
            if (res == null)
                return NotFound("Ther is no title with this id");
            return Ok(res);
        }
        [HttpGet("longest")]
        public async Task<ActionResult> GetLongest (string codeLine)
        {
            var variablesIds = await _titleService.GetVariableIdsFromCode(codeLine);
            var res = await _titleService.GetLongestJsonDataId(variablesIds);
            return Ok(res);
        }

    }
}
