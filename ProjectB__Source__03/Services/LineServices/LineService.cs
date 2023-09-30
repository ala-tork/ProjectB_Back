using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models.DTOS;
using ProjectB.Services.DtataFomJsonServices;
using StageTest.Models;
using StageTest.Models.ContainerLineFolder;
using StageTest.Services.TitleServices;
using StageTest.Services.VariableServices;
using System.Text.RegularExpressions;

namespace StageTest.Services.LineServices
{
    public class LineService : ILineService
    {
        private readonly ApplicationDbContext _Context;
        private readonly ITitleService _TitleService;
        private readonly IMapper _mapper;
        private readonly IVariableService _variableService;
        private readonly IDataFromJsonService _dataFromJsonService;
        public LineService(
            IVariableService variableService,
            ApplicationDbContext context,
            ITitleService titleService,
            IMapper mapper,
            IDataFromJsonService dataFromJsonService
            )
        {
            _variableService = variableService;
            _Context = context;
            _TitleService = titleService;
            _mapper= mapper;
            _dataFromJsonService = dataFromJsonService;
        }

        public async Task<ContainersLine> CreateLine(CreateLine lineDTO)
        {
            var line = _mapper.Map<ContainersLine>( lineDTO );
            await _Context.ContainersLines.AddAsync(line);
            await _Context.SaveChangesAsync();
            return line;
        }
        private readonly string GetValuesFromJsonURl = "https://localhost:7047/api/DataFromJson/var/";

        //public async Task<List<string>> GetallContainerLinesTite(string containerId)
        //{
        //    List<string> titles = new List<string>();

        //    if (!Guid.TryParse(containerId, out var id))
        //    {
        //        throw new ArgumentException("Invalid container ID");
        //    }

        //    var lines = await _Context.ContainersLines
        //        .Where(l => l.IdContainer == id)
        //        .OrderBy(l => l.LineNumber)
        //        .ToListAsync();

        //    foreach (var item in lines)
        //    {
        //        var title = await LinesTitles(item.IdContainerLine.ToString());
        //        titles.Add(title);
        //    }

        //    return titles;
        //}


        //public async Task<string> GetLinesTitle(string id)
        //{
        //    var line = await GetLineById(id);
        //    if (line == null)
        //    {
        //        return null;
        //    }

        //    return await _TitleService.GetVariableByLineCode(line.Code);

        //}
        public async Task<List<ContainersLine>> GetAllLines()
        {
            var res = await _Context.ContainersLines.ToListAsync();
            return res;
        }

        public async Task<ContainersLine> GetLineById(string id)
        {
            Guid.TryParse(id, out var LineId);
            var res = await _Context.ContainersLines.FirstOrDefaultAsync(l=>l.IdContainerLine==LineId);
            return res;
        }

        // get Lines Id By Code @@__  __@@
        public async Task<List<ContainersLine>> GetLinesIds(string code)
        {
            string containerid = null;

            string pattern = @"@@__(.*?)__@@";
            Match matches = Regex.Match(code, pattern);
            if(matches.Success)
            {
                containerid = matches.Groups[1].Value;
                if (Guid.TryParse(containerid, out var id))
                {
                    var lines = await _Context.ContainersLines.Where(l => l.IdContainer == id)
                        .OrderBy(l=>l.LineNumber)
                        .ToListAsync();
                    return lines;
                }
                return null;
            }
            else {return null; }
        }


        // get all Line Titles 
        public async Task<List<string>> LinesTitles(string id)
        {
            var line = await GetLineById(id);
            string title = null;
            List<string> titles = new List<string>();

            if (line == null)
            {
                return null;
            }

            title = await _TitleService.GetVariableByLineCode(line.Code);

            if (title.StartsWith("@@__"))
            {
                var lines = await GetLinesIds(title);
                title = null;

                foreach (var item in lines)
                {

                    var subTitle = await LinesTitles(item.IdContainerLine.ToString());

                    if (subTitle != null)
                    {
                        titles.AddRange(subTitle);
                    }
                }

                return titles;
            }
            else
            {
                if (title.StartsWith("Tab[") && title.EndsWith("]"))
                {
                    int startIndex = title.IndexOf("Tab[");
                    int endIndex = title.IndexOf("]", startIndex);
                    if (startIndex != -1 && endIndex != -1)
                    {
                        string tabContent = title.Substring(4, endIndex - 4);
                        string[] tabElements = tabContent.Split(',');

                        List<string> tabList = new List<string>(tabElements);
                        foreach (string element in tabList)
                        {
                            titles.Add(element.Replace(" ",""));
                        }
                    }
                }
                else
                {
                    titles.Add(title);
                }
                
                return titles;
            }
        }

        public async Task<List<string>> GetallContainerLinesTite(string containerId)
        {
            List<string> titles = new List<string>();

            if (!Guid.TryParse(containerId, out var id))
            {
                throw new ArgumentException("Invalid container ID");
            }

            var lines = await _Context.ContainersLines
                .Where(l => l.IdContainer == id)
                .OrderBy(l => l.LineNumber)
                .ToListAsync();

            foreach (var item in lines)
            {
                var title = await LinesTitles(item.IdContainerLine.ToString());
                titles.AddRange(title);
            }

            return titles;
        }

        public async Task<ContainersLine> DeleteLine(string id)
        {
            try
            {
                var line = await _Context.ContainersLines.FirstOrDefaultAsync(l => l.IdContainerLine == Guid.Parse(id));
                if (line == null)
                {
                    return null;
                }
                var variables = await _Context.ContainersVariables
                    .Where(v => v.IdContainerLine == line.IdContainerLine)
                    .ToListAsync();
                foreach (var item in variables)
                {
                    await _variableService.DeleteVariabelwithchildren(item.IdVariable.ToString());
                }
                _Context.ContainersLines.Remove(line);
                await _Context.SaveChangesAsync();
                return line;

            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> LinesTitlesFromVariabeType(string id)
        {
            var line = await GetLineById(id);
            string title = null;
            List<string> titles = new List<string>();

            if (line == null)
            {
                return null;
            }

            title = await _TitleService.GetVariableByLineCodeFromVariableType(line.Code);

            if (title.StartsWith("@@__"))
            {
                var lines = await GetLinesIds(title);
                title = null;

                foreach (var item in lines)
                {

                    var subTitle = await LinesTitlesFromVariabeType(item.IdContainerLine.ToString());

                    if (subTitle != null)
                    {
                        titles.AddRange(subTitle);
                    }
                }

                return titles;
            }
            else
            {
                var TitlesFromJson = await _dataFromJsonService.GetValuesFromJsonByKey(title);
                foreach (var jsonTitle in TitlesFromJson)
                {
                    foreach (var value in jsonTitle.values)
                    {
                        titles.Add(value);
                    }


                }
                
                return titles;
            }
        }

        public async Task<List<string>> GetallContainerLinesTiteFromJson(string containerId)
        {
            List<string> titles = new List<string>();

            if (!Guid.TryParse(containerId, out var id))
            {
                throw new ArgumentException("Invalid container ID");
            }

            var lines = await _Context.ContainersLines
                .Where(l => l.IdContainer == id)
                .OrderBy(l => l.LineNumber)
                .ToListAsync();

            foreach (var item in lines)
            {
                var title = await LinesTitlesFromVariabeType(item.IdContainerLine.ToString());
                titles.AddRange(title);
            }

            return titles;
        }





        //horizontal or vertical  using verification
        public async Task<List<string>> LinesTitlesFromVariabeTypeUsingIsVerticalAlign(string id)
        {
            var line = await GetLineById(id);

            if (line == null)
            {
                return null;
            }

            var Variabletitle = await _TitleService.GetLines(line.Code);


            List<string> titles = new List<string>();
            string HorizontalLine = null;

            if (line.IsVerticalAlign == true)
            {
                foreach (var item in Variabletitle)
                {
                    if (item.StartsWith("@@__"))
                    {
                        var lines = await GetLinesIds(item);

                        foreach (var l in lines)
                        {
                            var subTitle = await LinesTitlesFromVariabeTypeUsingIsVerticalAlign(l.IdContainerLine.ToString());

                            if (subTitle != null)
                            {
                                titles.AddRange(subTitle);
                            }
                        }
                    }
                    else
                    {
                        titles.Add(item);
                    }

                }
            }

            else if (line.IsVerticalAlign == false || line.IsVerticalAlign == null)
            {
                foreach (var item in Variabletitle)
                {
                    if (item.StartsWith("@@__"))
                    {
                        var lines = await GetLinesIds(item);

                        foreach (var l in lines)
                        {
                            var subTitle = await LinesTitlesFromVariabeTypeUsingIsVerticalAlign(l.IdContainerLine.ToString());

                            if (subTitle != null)
                            {
                                titles.AddRange(subTitle);
                            }
                        }
                    }
                    else
                    {
                        HorizontalLine += item.ToString();
                    }

                }
                titles.Add(HorizontalLine);
            }

            return titles;
        }


        public async Task<List<string>> GetallContainerLinesTiteFromJsonHorizontalOrVertical(string containerId)
        {
            List<string> titles = new List<string>();

            if (!Guid.TryParse(containerId, out var id))
            {
                throw new ArgumentException("Invalid container ID");
            }

            var lines = await _Context.ContainersLines
                .Where(l => l.IdContainer == id)
                .OrderBy(l => l.LineNumber)
                .ToListAsync();

            foreach (var item in lines)
            {
                var title = await LinesTitlesFromVariabeTypeUsingIsVerticalAlign(item.IdContainerLine.ToString());
                titles.AddRange(title);
            }

            return titles;
        }
        //public async Task<List<string>> LinesTitlesFromVariabeTypeUsingIsVerticalAlign(string id)
        //{
        //    var line = await GetLineById(id);

        //    if (line == null)
        //    {
        //        return null;
        //    }

        //    var Variabletitle = await _TitleService.GetVariableByLineCodeFromVariableTypeWithverification(line.Code);


        //    List<string> titles = new List<string>();
        //    string HorizontalLine = null;

        //    if (line.IsVerticalAlign == true )
        //    {
        //        foreach (var item in Variabletitle)
        //        {
        //            if (item.StartsWith("@@__"))
        //            {
        //                var lines = await GetLinesIds(item);

        //                foreach (var l in lines)
        //                {
        //                    var subTitle = await LinesTitlesFromVariabeTypeUsingIsVerticalAlign(l.IdContainerLine.ToString());

        //                    if (subTitle != null)
        //                    {
        //                        titles.AddRange(subTitle);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                titles.Add(item);
        //            }

        //        }
        //    }

        //    else if (line.IsVerticalAlign == false || line.IsVerticalAlign == null)
        //    {
        //        foreach (var item in Variabletitle)
        //        {
        //            if (item.StartsWith("@@__"))
        //            {
        //                var lines = await GetLinesIds(item);

        //                foreach (var l in lines)
        //                {
        //                    var subTitle = await LinesTitlesFromVariabeTypeUsingIsVerticalAlign(l.IdContainerLine.ToString());

        //                    if (subTitle != null)
        //                    {
        //                        titles.AddRange(subTitle);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                HorizontalLine += item.ToString();
        //            }

        //        }
        //        titles.Add(HorizontalLine);
        //    }

        //    return titles;
        //}


        //public async Task<List<string>> GetallContainerLinesTiteFromJsonHorizontalOrVertical(string containerId)
        //{
        //    List<string> titles = new List<string>();

        //    if (!Guid.TryParse(containerId, out var id))
        //    {
        //        throw new ArgumentException("Invalid container ID");
        //    }

        //    var lines = await _Context.ContainersLines
        //        .Where(l => l.IdContainer == id)
        //        .OrderBy(l => l.LineNumber)
        //        .ToListAsync();

        //    foreach (var item in lines)
        //    {
        //        var title = await LinesTitlesFromVariabeTypeUsingIsVerticalAlign(item.IdContainerLine.ToString());
        //        titles.AddRange(title);
        //    }

        //    return titles;
        //}
    }
}
