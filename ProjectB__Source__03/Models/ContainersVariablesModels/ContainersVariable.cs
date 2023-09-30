using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using StageTest.Models.ContainerLineFolder;
using StageTest.Models.ContainerModels;
using StageTest.Models.FolderModels;

namespace StageTest.Models.ContainersVariablesModels;

public partial class ContainersVariable
{
    public Guid IdVariable { get; set; }

    public Guid? IdParent { get; set; }

    public Guid? IdVariableType { get; set; }

    public Guid? IdContainerLine { get; set; }

    public Guid? IdContainerFolder { get; set; }

    public Guid? IdContainer { get; set; }

    public string? Title { get; set; }

    public string? TitleDynamic { get; set; }

    public int? Position { get; set; }

    public virtual ContainersFolder? IdContainerFolderNavigation { get; set; }

    public virtual ContainersLine? IdContainerLineNavigation { get; set; }

    public virtual Container? IdContainerNavigation { get; set; }
    [JsonIgnore]
    public virtual ContainersVariable? IdParentNavigation { get; set; }

    public virtual ContainersVariablesType? IdVariableTypeNavigation { get; set; }

    public virtual ICollection<ContainersVariable> InverseIdParentNavigation { get; set; } = new List<ContainersVariable>();
}
