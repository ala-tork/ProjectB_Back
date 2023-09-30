namespace ProjectB.Models.DTOS
{
    public class OutputContainerDto
    {
        public Guid IdContainer { get; set; }

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
