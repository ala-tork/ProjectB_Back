namespace ProjectB__Target__01.Models.ProjectVersionModels
{
    public class CreateProjectVersion
    {
        public Guid? IdProject { get; set; }

        public Guid? IdParent { get; set; }

        public Guid? PrototypeEditorPrototypesVersionsIdPrototypeVersion { get; set; }

        public string? Title { get; set; }

        public int? Version { get; set; }

        public string? Description { get; set; }

        public DateTime? Date { get; set; }

        public bool? IsFrontend { get; set; }

        public bool? IsBackend { get; set; }

        public bool? IsLastVersion { get; set; }

        public string? Time { get; set; }

        public string? ShortId { get; set; }

        public string? Path { get; set; }

        public bool? IsCrypted { get; set; }
    }
}
