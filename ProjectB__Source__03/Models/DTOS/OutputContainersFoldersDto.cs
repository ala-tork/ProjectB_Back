namespace ProjectB.Models.DTOS
{
    public class OutputContainersFoldersDto
    {
        public Guid IdContainerFolder { get; set; }

        public Guid? IdPrototypeVersion { get; set; }

        public Guid? IdParent { get; set; }

        public Guid? IdFolder { get; set; }

        public string? Title { get; set; }

        public string? TitleDynamic { get; set; }

        public bool? IsDynamicFolderName { get; set; }
    }
}
