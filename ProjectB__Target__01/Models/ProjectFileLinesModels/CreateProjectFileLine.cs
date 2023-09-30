namespace ProjectB__Target__01.Models.ProjectFileLinesModels
{
    public class CreateProjectFileLine
    {
        public Guid? IdProjectFile { get; set; }

        public int? LineNumber { get; set; }

        public string? Code { get; set; }

        public bool? IsCrypted { get; set; }
    }
}
