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
                else if (form.Key.Equals("DatabaseName") && !form.Value.Equals(string.Empty))
                {
                    procedureModel.DatabaseName = form.Value;
                }
                else if (form.Key.Equals("EntityName") && !form.Value.Equals(string.Empty))
                {
                    procedureModel.EntityName = form.Value;
                }

                if (form.Key.Equals("TypeOfProcedurePost"))
                {
                    if (procedureModel.TypeOfProcedure != null )
                    {
                        procedureModel.TypeOfProcedure = procedureModel.TypeOfProcedure + ", Post";
                    }
                    else
                    {
                        procedureModel.TypeOfProcedure = "Post";
                    }
                }
                if (form.Key.Equals("TypeOfProcedureGetAll"))
                {
                    if (procedureModel.TypeOfProcedure != null)
                    {
                        procedureModel.TypeOfProcedure = procedureModel.TypeOfProcedure + ", GetAll";
                    }
                    else
                    {
                        procedureModel.TypeOfProcedure = "GetAll";
                    }
                }
                if (form.Key.Equals("TypeOfProcedureGetById"))
                {
                    if (procedureModel.TypeOfProcedure != null)
                    {
                        procedureModel.TypeOfProcedure = procedureModel.TypeOfProcedure + ", GetById";
                    }
                    else
                    {
                        procedureModel.TypeOfProcedure = "GetById";
                    }
                }
                if (form.Key.Equals("TypeOfProcedureDelete"))
                {
                    if (procedureModel.TypeOfProcedure != null)
                    {
                        procedureModel.TypeOfProcedure = procedureModel.TypeOfProcedure + ", Delete";
                    }
                    else
                    {
                        procedureModel.TypeOfProcedure = "Delete";
                    }
                }
                if (form.Key.Equals("TypeOfProcedurePut"))
                {
                    if (procedureModel.TypeOfProcedure != null)
                    {
                        procedureModel.TypeOfProcedure = procedureModel.TypeOfProcedure + ", Put";
                    }
                    else
                    {
                        procedureModel.TypeOfProcedure = "Put";
                    }
                }
                if (form.Key.Equals("TypeOfProcedureAll"))
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

            if (procedureModel.TypeOfProcedure.Contains("GetAll"))
            {
                ret = TemplateGet(procedureModel);
            }


            if (procedureModel.TypeOfProcedure.Contains("Post"))
            {
                ret = ret + TemplatePost(procedureModel); 
            }



            return ret;

        }

        public static string TemplateGet(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "USE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\n\r\n\r\nCREATE PROCEDURE [dbo].[USP_" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "_";

            string paramsHeader = string.Empty;

            if (procedureModel.TypeOfProcedure.Contains("GetAll"))
            {
                paramsHeader = paramsHeader + "GetAll] \r\nAS\r\nBEGIN\r\n\tSET NOCOUNT ON;\r\n\tSELECT * \r\n\t\tFROM ";
            }

            paramsHeader = paramsHeader + procedureModel.DatabaseName + "_" + procedureModel.EntityName + " ORDER BY";

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        paramsHeader = paramsHeader + " " + item.Name + " ASC\r\n\tEND\r\nGO\r\n\t";
                    }

                }
            }
            
            ret = header + paramsHeader;

            return ret;
        }

        public static string TemplatePost(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "USE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE PROCEDURE [dbo].[USP_" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "_";

            if (procedureModel.TypeOfProcedure.Contains("Post"))
            {
                header = header + "Post](";
            }

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

            string final = ")\r\n\tSET NOCOUNT ON;\r\nEND\r\nGO\r\n\t";

            ret = header + paramsHeader + middle + paramsMiddle + baseboard + paramsBaseboard + final;

            return ret;
        }

    }
}
