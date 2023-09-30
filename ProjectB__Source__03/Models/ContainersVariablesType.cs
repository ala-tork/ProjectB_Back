using System;
using System.Collections.Generic;
using StageTest.Models.ContainersVariablesModels;

namespace StageTest.Models;

public partial class ContainersVariablesType
{
    public Guid IdVariableType { get; set; }

    public Guid? IdParent { get; set; }

    public string? Title { get; set; }

    //public bool? isVerticalAlign { get; set; }

    public virtual ICollection<ContainersVariable> ContainersVariables { get; set; } = new List<ContainersVariable>();

    public virtual ContainersVariablesType? IdParentNavigation { get; set; }

    public virtual ICollection<ContainersVariablesType> InverseIdParentNavigation { get; set; } = new List<ContainersVariablesType>();
}
