namespace ProjectB.Models.DTOS
{
    public class CreateVariable
    {
        public Guid? IdParent { get; set; }

        public Guid? IdVariableType { get; set; }

        public Guid? IdContainerLine { get; set; }

        public Guid? IdContainerFolder { get; set; }

        public Guid? IdContainer { get; set; }

        public string? Title { get; set; }

        public string? TitleDynamic { get; set; }

        public int? Position { get; set; }
    }
}
