namespace ProcedureGenerator.Web.Models
{
    public class ProcedureModel
    {
        public string? DatabaseLanguage { get; set; }
        public string? DatabaseName { get; set; }
        public string? EntityName { get; set; }
        public string? TypeOfProcedure { get; set; }
        public List<PropertiesModel>? listPropertiesModels { get;set; }
    }
}
