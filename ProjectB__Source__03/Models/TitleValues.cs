namespace ProjectB.Models
{
    public class TitleValues
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
        public List<TitleValues> Children { get; set; }
    }
    public class VariableData
    {
        public string ParentName { get; set; }
        public List<string> values { get; set; }
    }

}

