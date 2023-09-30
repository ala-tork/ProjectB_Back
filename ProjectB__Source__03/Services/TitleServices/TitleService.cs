using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models;
using ProjectB.Services.DtataFomJsonServices;
using StageTest.Models;
using System.Text.RegularExpressions;

namespace StageTest.Services.TitleServices
{
    public class TitleService : ITitleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataFromJsonService _dataFromJsonService;
        public TitleService(ApplicationDbContext context, IDataFromJsonService dataFromJsonService)
        {
            _context = context;
            _dataFromJsonService = dataFromJsonService;
        }
        //public async Task<string> GetVariableByLineCode(string CodeLine)
        //{
        //    string prefix1 = "@@__VAR__";
        //    string suffix1 = "__VAR__@@";
        //    string prefix2 = "@@__";
        //    string suffix2 = "__@@";
        //    string title = CodeLine;

        //    List<string> VariableIds = await GetVariableIdsFromCode(CodeLine);

        //    foreach (var item in VariableIds)
        //    {
        //        string variableId = null;

        //        if (item.StartsWith(prefix1) && item.EndsWith(suffix1))
        //        {
        //            variableId = item.Substring(prefix1.Length, item.Length - (prefix1.Length + suffix1.Length));
        //        }
        //        else if (item.StartsWith(prefix2) && item.EndsWith(suffix2))
        //        {
        //            variableId = item.Substring(prefix2.Length, item.Length - (prefix2.Length + suffix2.Length));
        //        }

        //        if (variableId != null && Guid.TryParse(variableId, out var idString))
        //        {
        //            var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == idString);

        //            if (containerVariable != null)
        //            {
        //                title = title.Replace(item, containerVariable.Title);
        //            }
        //        }
        //    }

        //    return title;
        //}


        //public async Task<List<string>> GetVariableIdsFromCode(string code)
        //{
        //    List<string> variablesIds = new List<string>();
        //    string pattern = @"@@(?:__VAR__(.*?)__VAR__|__(.*?)__)@@";
        //    MatchCollection matches = Regex.Matches(code, pattern);

        //    foreach (Match match in matches)
        //    {
        //        variablesIds.Add(match.Value);
        //    }

        //    return variablesIds;
        //}



        // Get Titles from code @@__Var__   __VAR__@@

        public async Task<string> GetVariableByCode(string CodeLine)
        {
            string prefix1 = "@@__VAR__";
            string suffix1 = "__VAR__@@";
            string title = CodeLine;

            List<string> VariableIds = await GetVariableIdsFromCode(CodeLine);

            foreach (var item in VariableIds)
            {
                string variableId = null;

                if (item.StartsWith(prefix1) && item.EndsWith(suffix1))
                {
                    variableId = item.Substring(prefix1.Length, item.Length - (prefix1.Length + suffix1.Length));
                }

                if (variableId != null && Guid.TryParse(variableId, out var idString))
                {
                    var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == idString);


                    if (containerVariable != null)
                    {
                        title = title.Replace(item, containerVariable.Title);
                    }
                }
            }

            return title;
        }
        public async Task<List<string>> GetVariableIdsFromCode(string code)
        {
            List<string> variablesIds = new List<string>();

            string pattern = @"@@__VAR__(.*?)__VAR__@@";
            MatchCollection matches = Regex.Matches(code, pattern);

            foreach (Match match in matches)
            {
                variablesIds.Add(match.Value);
            }

            return variablesIds;
        }

        //using variabe table 
        public async Task<string> GetVariableByLineCode(string CodeLine)
        {
            string prefix1 = "@@__VAR__";
            string suffix1 = "__VAR__@@";
            string title = CodeLine;

            List<string> VariableIds = await GetVariableIdsFromCode(CodeLine);

            foreach (var item in VariableIds)
            {
                string variableId = null;

                if (item.StartsWith(prefix1) && item.EndsWith(suffix1))
                {
                    variableId = item.Substring(prefix1.Length, item.Length - (prefix1.Length + suffix1.Length));
                }

                if (variableId != null && Guid.TryParse(variableId, out var idvariable))
                {
                    var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == idvariable);

                    if (containerVariable != null)
                    {
                        title = title.Replace(item, containerVariable.Title);
                    }
                }
            }

            return title;
        }



        // using variabletype table
        public async Task<string> GetVariableByLineCodeFromVariableType(string CodeLine)
        {
            string prefix1 = "@@__VAR__";
            string suffix1 = "__VAR__@@";
            string title = CodeLine;

            List<string> VariableIds = await GetVariableIdsFromCode(CodeLine);

            foreach (var item in VariableIds)
            {
                string variableId = null;

                if (item.StartsWith(prefix1) && item.EndsWith(suffix1))
                {
                    variableId = item.Substring(prefix1.Length, item.Length - (prefix1.Length + suffix1.Length));
                }

                if (variableId != null && Guid.TryParse(variableId, out var idvariable))
                {
                    var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == idvariable);

                    var containervariableType = await _context.ContainersVariablesTypes
                                   .FirstOrDefaultAsync(vt => vt.IdVariableType == containerVariable.IdVariableType);

                    if (containervariableType != null)
                    {
                        title = title.Replace(item, containervariableType.Title);
                    }
                }
            }

            return title;
        }


        // using variabletype table isveertical
        public async Task<List<VariableIsVerticalModel>> GetVariableIsVerticalByLineCodeFromVariableType(string CodeLine)
        {
            string prefix1 = "@@__VAR__";
            string suffix1 = "__VAR__@@";
            string title = CodeLine;

            List<string> VariableIds = await GetVariableIdsFromCode(CodeLine);
            List<VariableIsVerticalModel> variablestitles = new List<VariableIsVerticalModel>();
            foreach (var item in VariableIds)
            {
                string variableId = null;

                if (item.StartsWith(prefix1) && item.EndsWith(suffix1))
                {
                    variableId = item.Substring(prefix1.Length, item.Length - (prefix1.Length + suffix1.Length));
                }

                if (variableId != null && Guid.TryParse(variableId, out var idvariable))
                {
                    var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == idvariable);

                    var containervariableType = await _context.ContainersVariablesTypes
                                   .FirstOrDefaultAsync(vt => vt.IdVariableType == containerVariable.IdVariableType);

                    if (containervariableType != null)
                    {
                        //title = title.Replace(item, containervariableType.Title);
                        var variable = new VariableIsVerticalModel
                        {
                            VariableName = containervariableType.Title,
                            //IsVertical = containervariableType.isVerticalAlign
                        };
                        variablestitles.Add(variable);
                        //return variablestitles;
                    }
                }
            }

            return variablestitles;
        }


        //using variabletype table and Json File


        public async Task<List<string>> GetVariableByLineCodeFromVariableTypeWithverification(string CodeLine)
        {
            string prefix1 = "@@__VAR__";
            string suffix1 = "__VAR__@@";
            string title = CodeLine;

            List<string> titlesList = new List<string>();

            List<string> VariableIds = await GetVariableIdsFromCode(CodeLine);
            if (VariableIds.Any())
            {
                foreach (var item in VariableIds)
                {
                    string variableId = null;

                    if (item.StartsWith(prefix1) && item.EndsWith(suffix1))
                    {
                        variableId = item.Substring(prefix1.Length, item.Length - (prefix1.Length + suffix1.Length));
                    }

                    if (variableId != null && Guid.TryParse(variableId, out var idvariable))
                    {
                        var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == idvariable);

                        if (containerVariable != null)
                        {
                            var containervariableType = await _context.ContainersVariablesTypes
                                .FirstOrDefaultAsync(vt => vt.IdVariableType == containerVariable.IdVariableType);

                            if (containervariableType != null)
                            {
                                var jsonData = await _dataFromJsonService.GetValuesFromJsonByKey(containervariableType.Title);

                                foreach (var titlesValues in jsonData)
                                {
                                    foreach (var t in titlesValues.values)
                                    {
                                        var res = title.Replace(item, t);
                                        titlesList.Add(res);
                                    }
                                }
                            }
                        }
                    }

                }



                return titlesList;
            }
            else
            {
                titlesList.Add(CodeLine);
                return titlesList;
            }

        }



        public async Task<string> GetLongestJsonDataId(List<string> idlist)
        {
            string prefix1 = "@@__VAR__";
            string suffix1 = "__VAR__@@";

            int aux = 0;
            string idResult = null;

            foreach (var item in idlist)
            {
                string variableId = null;

                if (item.StartsWith(prefix1) && item.EndsWith(suffix1))
                {
                    variableId = item.Substring(prefix1.Length, item.Length - (prefix1.Length + suffix1.Length));

                    try
                    {
                        var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == Guid.Parse(variableId));
                        if (containerVariable != null)
                        {
                            var containerVariableType = await _context.ContainersVariablesTypes
                                .FirstOrDefaultAsync(vt => vt.IdVariableType == containerVariable.IdVariableType);

                            if (containerVariableType != null)
                            {
                                var jsonData = await _dataFromJsonService.GetValuesFromJsonByKey(containerVariableType.Title);

                                int nbvalue = 0;
                                foreach (var titlesValues in jsonData)
                                {
                                    foreach (var t in titlesValues.values)
                                    {
                                        nbvalue++;
                                    }
                                }

                                if (nbvalue > aux)
                                {
                                    idResult = variableId;
                                    aux = nbvalue;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return "@@__VAR__"+idResult+"__VAR__@@";
        }


        public async Task<List<string>> GetLines(string CodeLine)
        {
            string prefix1 = "@@__VAR__";

            string suffix1 = "__VAR__@@";

            string title = CodeLine;

            List<string> VariableIds = await GetVariableIdsFromCode(CodeLine);

            string idVariableWithLongestData = await GetLongestJsonDataId(VariableIds);

            List<string> titlesList = new List<string>();

            if (VariableIds.Any())
            {

                    string variableId = null;

                    if (idVariableWithLongestData.StartsWith(prefix1) && idVariableWithLongestData.EndsWith(suffix1))
                    {
                        variableId = idVariableWithLongestData.Substring(prefix1.Length, idVariableWithLongestData.Length - (prefix1.Length + suffix1.Length));
                    }

                    if (variableId != null && Guid.TryParse(variableId, out var idvariable))
                    {
                        var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == idvariable);

                        if (containerVariable != null)
                        {
                            var containervariableType = await _context.ContainersVariablesTypes
                                .FirstOrDefaultAsync(vt => vt.IdVariableType == containerVariable.IdVariableType);

                            if (containervariableType != null)
                            {
                                var jsonData = await _dataFromJsonService.GetValuesFromJsonByKey(containervariableType.Title);

                                foreach (var titlesValues in jsonData)
                                {
                                    foreach (var t in titlesValues.values)
                                    {
                                        var res0 = title.Replace(idVariableWithLongestData, t);
                                        var res1 = await makeTitle(t,res0);
                                        titlesList.Add(res1);
                                    }
                                }
                            }
                        }
                    }

                return titlesList;
            }
            else
            {
                titlesList.Add(CodeLine);
                return titlesList;
            }

        }

        public async Task<string> makeTitle(string value, string unCompleteTitle)
        {
            string prefix1 = "@@__VAR__";
            string suffix1 = "__VAR__@@";

            var jsonData = await _dataFromJsonService.GetValuesFromJsonByKey(value);

            List<string> VariableIds = await GetVariableIdsFromCode(unCompleteTitle);

            var line = unCompleteTitle;
            var changed = false;
            foreach (var item in VariableIds)
            {
                string variableId = null;

                if (item.StartsWith(prefix1) && item.EndsWith(suffix1))
                {
                    variableId = item.Substring(prefix1.Length, item.Length - (prefix1.Length + suffix1.Length));
                }
                if (variableId != null && Guid.TryParse(variableId, out var idvariable))
                {
                    var containerVariable = await _context.ContainersVariables.FirstOrDefaultAsync(v => v.IdVariable == idvariable);
                    if (containerVariable != null)
                    {
                        var containervariableType = await _context.ContainersVariablesTypes
                                    .FirstOrDefaultAsync(vt => vt.IdVariableType == containerVariable.IdVariableType);

                        var values = await _dataFromJsonService.GetValuesFromJsonByKey(containervariableType.Title);

                        if (containervariableType != null)
                        {
                            foreach (var data in values)
                            {
                                if (data.ParentName == value)
                                {
                                    changed = true;
                                    foreach (var d in data.values)
                                    {
                                        line = line.Replace(item, d);
                                    }
                                }
                                else
                                {
                                    foreach (var json in jsonData)
                                    {
                                        foreach (var d in data.values)
                                        {
                                            if (json.ParentName == d)
                                            {
                                                changed = true;

                                                line = line.Replace(item, d);

                                            }
                                        }


                                    }
                                }

                            }

                            if (!changed) { line = line.Replace(item,""); }
                        }
                    }

                }
            }
            return line;
        }
    }
}