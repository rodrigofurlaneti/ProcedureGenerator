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

            if (procedureModel.TypeOfProcedure.Equals("All"))
            {
                ret = TemplateCreateTable(procedureModel);
                ret = ret + TemplateDelete(procedureModel);
                ret = ret + TemplateGet(procedureModel);
                ret = ret + TemplateGetById(procedureModel);
                ret = ret + TemplatePost(procedureModel);
                ret = ret + TemplatePut(procedureModel);
            }
            else
            {
                if (procedureModel.TypeOfProcedure.Contains("CreateTable"))
                {
                    ret = TemplateCreateTable(procedureModel);
                }

                if (procedureModel.TypeOfProcedure.Contains("Delete"))
                {
                    ret = TemplateDelete(procedureModel);
                }

                if (procedureModel.TypeOfProcedure.Contains("GetAll"))
                {
                    ret = ret + TemplateGet(procedureModel);
                }

                if (procedureModel.TypeOfProcedure.Contains("GetById"))
                {
                    ret = ret + TemplateGetById(procedureModel);
                }

                if (procedureModel.TypeOfProcedure.Contains("Post"))
                {
                    ret = ret + TemplatePost(procedureModel);
                }

                if (procedureModel.TypeOfProcedure.Contains("Put"))
                {
                    ret = ret + TemplatePut(procedureModel);
                }
            }

            return ret;

        }

        public static string TemplateCreateTable(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "USE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE TABLE [dbo].[" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "]\r\n\t(";

            string paramsHeader = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = "[" + item.Name + "] [INT] IDENTITY(1,1) NOT NULL,\r\n\t";
                        }
                    }
                    if (!item.Name.Equals(first.Name))
                    {
                        if (item.Type.Equals("Text"))
                        {
                            paramsHeader = paramsHeader + "[" + item.Name + "] [VARCHAR]("+ item.Size + ") NULL,\r\n\t";
                        }
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = paramsHeader +"[" + item.Name + "] [INT] NULL,\r\n\t";
                        }
                        if (item.Type.Equals("True or false"))
                        {
                            paramsHeader = paramsHeader + "[" + item.Name + "] [BIT] NULL,\r\n\t";
                        }
                        if (item.Type.Equals("Date and time"))
                        {
                            paramsHeader = paramsHeader + "[" + item.Name + "] [DATETIME] NULL,\r\n\t";
                        }
                    }
                }
            }

            var middle = "CONSTRAINT [PK_"+ procedureModel.DatabaseName + "_"+ procedureModel.EntityName + "] PRIMARY KEY CLUSTERED\r\n\t(";

            string final = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        if (item.Type.Equals("Number"))
                        {
                            final = "["+ item.Name + "] ASC\r\n\t)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]\r\n\t) ON [PRIMARY]\r\n\tGO\r\n\t";
                        }
                    }
                }
            }

            ret = header + paramsHeader + middle + final;

            return ret;
        }

        public static string TemplateDelete(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "\r\nUSE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE PROCEDURE [dbo].[USP_" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "_";

            if (procedureModel.TypeOfProcedure.Contains("Delete") || procedureModel.TypeOfProcedure.Equals("All"))
            {
                header = header + "Delete]\r\n\t\t(";
            }

            string paramsHeader = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = "@" + item.Name + " INT";
                        }
                    }

                }
            }

            var middle = ")\r\nAS\r\nBEGIN\r\n\tDELETE FROM [dbo].[" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "]\r\n\t\tWHERE\r\n\t\t\t";

            string paramsMiddle = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        if (item.Type.Equals("Number"))
                        {
                            paramsMiddle = item.Name + " = @" + item.Name + ";";
                        }
                    }

                }
            }

            string final = "\r\n\tSET NOCOUNT ON;\r\nEND\r\nGO\r\n\t";

            ret = header + paramsHeader + middle + paramsMiddle + final;

            return ret;
        }


        public static string TemplateGet(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "\r\nUSE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\n\r\n\r\nCREATE PROCEDURE [dbo].[USP_" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "_";

            string paramsHeader = string.Empty;

            if (procedureModel.TypeOfProcedure.Contains("GetAll") || procedureModel.TypeOfProcedure.Equals("All"))
            {
                paramsHeader = "GetAll] \r\nAS\r\nBEGIN\r\n\tSET NOCOUNT ON;\r\n\tSELECT * \r\n\t\tFROM ";
            }

            string middle = string.Empty;

            middle = procedureModel.DatabaseName + "_" + procedureModel.EntityName + "\r\n\t\t\tORDER BY";

            string paramsMiddle = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        paramsMiddle = "\r\n\t\t\t\t" + item.Name + " ASC\r\n\tEND\r\nGO\r\n\t";
                    }

                }
            }
            
            ret = header + paramsHeader + middle + paramsMiddle;

            return ret;
        }

        public static string TemplateGetById(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "\r\nUSE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE PROCEDURE [dbo].[USP_" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "_";

            if (procedureModel.TypeOfProcedure.Contains("GetById") || procedureModel.TypeOfProcedure.Equals("All"))
            {
                header = header + "GetById]\r\n\t\t(";
            }

            string paramsHeader = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = "@" + item.Name + " INT";
                        }
                    }

                }
            }

            var middle = ")\r\nAS\r\nBEGIN\r\n\tSELECT TOP 1 * FROM [dbo].[" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "]\r\n\t\tWHERE\r\n\t\t\t";

            string paramsMiddle = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        if (item.Type.Equals("Number"))
                        {
                            paramsMiddle = item.Name + " = @" + item.Name + ";";
                        }
                    }

                }
            }

            string final = "\r\n\tSET NOCOUNT ON;\r\nEND\r\nGO\r\n\t";

            ret = header + paramsHeader + middle + paramsMiddle + final;

            return ret;
        }

        public static string TemplatePost(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "\r\nUSE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE PROCEDURE [dbo].[USP_" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "_";

            if (procedureModel.TypeOfProcedure.Contains("Post") || procedureModel.TypeOfProcedure.Equals("All"))
            {
                header = header + "Post]\r\n\t(";
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
                            paramsHeader = paramsHeader + "@" + item.Name + " VARCHAR(" + item.Size + "),\r\n\t";
                        }
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " INT,\r\n\t";
                        }
                        if (item.Type.Equals("True or false"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " BIT,\r\n\t";
                        }
                        if (item.Type.Equals("Date and time"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " DATETIME,\r\n\t";
                        }
                    }
                    if (item.Name.Equals(last.Name))
                    {
                        if (item.Type.Equals("Text"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " VARCHAR(" + item.Size + "))\r\n\t";
                        }
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " INT)\r\n\t";
                        }
                        if (item.Type.Equals("True or false"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " BIT)\r\n\t";
                        }
                        if (item.Type.Equals("Date and time"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " DATETIME)\r\n\t";
                        }
                    }

                }
            }

            var middle = "\r\nAS\r\nBEGIN\r\n\tINSERT INTO [dbo].[" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "]\r\n\t(";

            string paramsMiddle = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var last = procedureModel.listPropertiesModels.Last();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (!item.Name.Equals(last.Name))
                    {
                        paramsMiddle = paramsMiddle + item.Name + ",\r\n\t";
                    }
                    if (item.Name.Equals(last.Name))
                    {
                        paramsMiddle = paramsMiddle + item.Name +")\r\n\t";
                    }
                }
            }

            string baseboard = "\r\nVALUES\r\n\t(";

            string paramsBaseboard = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var last = procedureModel.listPropertiesModels.Last();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (!item.Name.Equals(last.Name))
                    {
                        paramsBaseboard = paramsBaseboard + "@" + item.Name + ",\r\n\t";
                    }
                    if (item.Name.Equals(last.Name))
                    {
                        paramsBaseboard = paramsBaseboard + "@" + item.Name + ")\r\n\t";
                    }
                }
            }

            string final = "\r\n\tSET NOCOUNT ON;\r\nEND\r\nGO\r\n\t";

            ret = header + paramsHeader + middle + paramsMiddle + baseboard + paramsBaseboard + final;

            return ret;
        }

        public static string TemplatePut(ProcedureModel procedureModel)
        {
            string ret = string.Empty;

            string header = "\r\nUSE [" + procedureModel.DatabaseName + "]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE PROCEDURE [dbo].[USP_" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "_";

            if (procedureModel.TypeOfProcedure.Contains("Put") || procedureModel.TypeOfProcedure.Equals("All"))
            {
                header = header + "Put]\r\n\t(";
            }

            string paramsHeader = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var last = procedureModel.listPropertiesModels.Last();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (!item.Name.Equals(last.Name))
                    {
                        if (item.Type.Equals("Text"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " VARCHAR(" + item.Size + "),\r\n\t";
                        }
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " INT,\r\n\t";
                        }
                        if (item.Type.Equals("True or false"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " BIT,\r\n\t";
                        }
                        if (item.Type.Equals("Date and time"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " DATETIME,\r\n\t";
                        }
                    }
                    if (item.Name.Equals(last.Name))
                    {
                        if (item.Type.Equals("Text"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " VARCHAR(" + item.Size + "))\r\n\t";
                        }
                        if (item.Type.Equals("Number"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " INT)\r\n\t";
                        }
                        if (item.Type.Equals("True or false"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " BIT)\r\n\t";
                        }
                        if (item.Type.Equals("Date and time"))
                        {
                            paramsHeader = paramsHeader + "@" + item.Name + " DATETIME)\r\n\t";
                        }
                    }

                }
            }

            var middle = "\r\nAS\r\nBEGIN\r\n\tUPDATE [dbo].[" + procedureModel.DatabaseName + "_" + procedureModel.EntityName + "]\r\n\t";

            string paramsMiddle = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                var last = procedureModel.listPropertiesModels.Last();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (!item.Name.Equals(first.Name) && !item.Name.Equals(last.Name) && item.Equals(procedureModel.listPropertiesModels[1]))
                    {
                        paramsMiddle = paramsMiddle + " SET " + item.Name + " = @" + item.Name + ",\r\n\t\t";
                    }
                    if (!item.Name.Equals(first.Name) && !item.Name.Equals(last.Name) && !item.Equals(procedureModel.listPropertiesModels[1]))
                    {
                        paramsMiddle = paramsMiddle + item.Name + " = @" + item.Name + ",\r\n\t\t";
                    }
                    if (item.Name.Equals(last.Name))
                    {
                        paramsMiddle = paramsMiddle + item.Name + " = @" + item.Name + "\r\n\t\t";
                    }

                }
            }

            string baseboard = "\r\n\tWHERE\r\n\t\t\t\t";

            string paramsBaseboard = string.Empty;

            if (procedureModel.listPropertiesModels.Count > 0)
            {
                var first = procedureModel.listPropertiesModels.First();
                foreach (var item in procedureModel.listPropertiesModels)
                {
                    if (item.Name.Equals(first.Name))
                    {
                        paramsBaseboard = item.Name + " = @" + item.Name + ";\r\n\t\t\t\t";
                    }
                }
            }

            string final = "\r\n\tSET NOCOUNT ON;\r\nEND\r\nGO\r\n\t";

            ret = header + paramsHeader + middle + paramsMiddle + baseboard + paramsBaseboard + final;

            return ret;
        }

    }
}
