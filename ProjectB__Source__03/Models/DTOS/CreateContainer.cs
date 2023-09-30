namespace ProjectB.Models.DTOS
{
    public class CreateContainer
    {
        public Guid? IdParent { get; set; }

        public Guid? IdContainerFolder { get; set; }

        public Guid? IdFile { get; set; }

        public string? Title { get; set; }

        public string? TitleDynamic { get; set; }

        public bool? IsDynamicFileName { get; set; }

        public bool? IsDynamicContent { get; set; }

        public string? Extension { get; set; }
    }
}
