namespace ProjectB__Target__01.Models.ProjectVersionModels
{
    public class PrototypeVersionInfo
    {
        public Guid IdPrototypeVersion { get; set; }

        public Guid? IdPrototype { get; set; }

        public string? Title { get; set; }

        public int? Version { get; set; }

        public string? Description { get; set; }

        public DateTime? Date { get; set; }

        public bool? IsFrontend { get; set; }

        public bool? IsBackend { get; set; }

        public bool? IsLastVersion { get; set; }

        public TimeSpan? Time { get; set; }

        public string? ShortId { get; set; }

        public string? Path { get; set; }
    }
}
