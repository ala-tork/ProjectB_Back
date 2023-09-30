namespace ProjectB__Target__01.Models.ProjectModels
{
    public class CreateProject
    {
        public Guid IdProject { get; set; }

        public string? Title { get; set; }

        public string? Version { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? Time { get; set; }

        public string? ShortId { get; set; }
    }
}
