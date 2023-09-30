using System;
using System.Collections.Generic;
using StageTest.Models.ContainerModels;
using StageTest.Models.ContainersVariablesModels;

namespace StageTest.Models.ContainerLineFolder;

public partial class ContainersLine
{
    public Guid IdContainerLine { get; set; }

    public Guid? IdContainer { get; set; }

    public int? LineNumber { get; set; }

    public string? Code { get; set; }

    public bool? IsVerticalAlign { get; set; }

    public virtual ICollection<ContainersVariable> ContainersVariables { get; set; } = new List<ContainersVariable>();

    public virtual Container? IdContainerNavigation { get; set; }
}
