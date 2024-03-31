using ProcedureGenerator.Web.Models;
using System.Text.Json;

namespace ProcedureGenerator.Web.Services
{
    public static class HomeService
    {
        public static ProcedureModel  DeparaController(IFormCollection iFormCollection)
        {
            ProcedureModel procedureModel = new ProcedureModel();

            foreach (var form in iFormCollection) {

                if (form.Key.Equals("DatabaseLanguage") && form.Value.Equals("1"))
                {
                    procedureModel.DatabaseLanguage = "Sql";
                }
                else if (form.Key.Equals("DatabaseLanguage") && form.Value.Equals("2"))
                {
                    procedureModel.DatabaseLanguage = "Pl Sql";
                }
                else if (form.Key.Equals("DatabaseLanguage") && form.Value.Equals("3"))
                {
                    procedureModel.DatabaseLanguage = "My Sql";
                }
                else if (form.Key.Equals("DatabaseName") && !form.Value.Equals(string.Empty))
                {
                    procedureModel.DatabaseName = form.Value;
                }
                else if (form.Key.Equals("EntityName") && !form.Value.Equals(string.Empty))
                {
                    procedureModel.EntityName = form.Value;
                }
                else if (form.Key.Equals("TypeOfProcedurePost"))
                {
                    procedureModel.TypeOfProcedure = "Post";
                }
                else if (form.Key.Equals("TypeOfProcedureGetAl"))
                {
                    procedureModel.TypeOfProcedure = "GetAll";
                }
                else if (form.Key.Equals("TypeOfProcedureGetById"))
                {
                    procedureModel.TypeOfProcedure = "GetById";
                }
                else if (form.Key.Equals("TypeOfProcedureDelete"))
                {
                    procedureModel.TypeOfProcedure = "Delete";
                }
                else if (form.Key.Equals("TypeOfProcedurePut"))
                {
                    procedureModel.TypeOfProcedure = "Put";
                }
                else if (form.Key.Equals("TypeOfProcedureAll"))
                {
                    procedureModel.TypeOfProcedure = "All";
                }
                else if (form.Key.Equals("InputListProperties"))
                {
                    var itens = JsonSerializer.Deserialize<List<PropertiesModel>>(form.Value);

                    if(itens.Count > 0)
                    {
                        procedureModel.listPropertiesModels = new List<PropertiesModel>(); 

                        foreach (var item in itens)
                        {
                            procedureModel.listPropertiesModels.Add(item);
                        }
                    }
                }

            }

            return procedureModel;
        }

        public static string Template(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            if (procedureModel.TypeOfProcedure.Equals("Post"))
            {
                ret = TemplatePost(procedureModel); 
            }

            return ret;

        }

        public static string TemplatePost(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "USE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE PROCEDURE [dbo].[USP_" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "_" + procedureModel.TypeOfProcedure + "](";
            
            string paramsHeader = string.Empty;

            if(procedureModel.listPropertiesModels.Count > 0)
            {
                var last = procedureModel.listPropertiesModels.Last();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (!item.Name.Equals(last.Name))
                    {
                        if (item.Type.Equals("Text"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " VARCHAR(" + item.Size + "),";
                        }
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " INT,";
                        }
                        if (item.Type.Equals("True or false"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " BIT,";
                        }
                        if (item.Type.Equals("Date and time"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " DATETIME,";
                        }
                    }
                    if (item.Name.Equals(last.Name))
                    {
                        if (item.Type.Equals("Text"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " VARCHAR(" + item.Size + ")";
                        }
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " INT";
                        }
                        if (item.Type.Equals("True or false"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " BIT";
                        }
                        if (item.Type.Equals("Date and time"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " DATETIME";
                        }
                    }

                }
            }

            var middle = ")\r\nAS\r\nBEGIN\r\n\tINSERT INTO [dbo].[" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "] (";

            string paramsMiddle = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var last = procedureModel.listPropertiesModels.Last();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (!item.Name.Equals(last.Name))
                    {
                        paramsMiddle = paramsMiddle + item.Name + ",";
                    }
                    if (item.Name.Equals(last.Name))
                    {
                        paramsMiddle = paramsMiddle + item.Name;
                    }
                }
            }

            string baseboard = ")\r\nVALUES\r\n\t(";

            string paramsBaseboard = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var last = procedureModel.listPropertiesModels.Last();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (!item.Name.Equals(last.Name))
                    {
                        paramsBaseboard = paramsBaseboard + "@" + item.Name + ",";
                    }
                    if (item.Name.Equals(last.Name))
                    {
                        paramsBaseboard = paramsBaseboard + "@" + item.Name;
                    }
                }
            }

            string final = ")\r\n\tSET NOCOUNT ON;\r\nEND\r\nGO";

            ret = header + paramsHeader + middle + paramsMiddle + baseboard + paramsBaseboard + final;

            return ret;
        }

    }
}
