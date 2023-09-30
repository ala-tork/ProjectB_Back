using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using StageTest.Models.ContainerLineFolder;
using StageTest.Models.ContainersVariablesModels;
using StageTest.Models.FolderModels;

namespace StageTest.Models.ContainerModels;

public partial class Container
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

    public virtual ICollection<ContainersLine> ContainersLines { get; set; } = new List<ContainersLine>();

    public virtual ICollection<ContainersVariable> ContainersVariables { get; set; } = new List<ContainersVariable>();

    public virtual ContainersFolder? IdContainerFolderNavigation { get; set; }

    public virtual Container? IdParentNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<Container> InverseIdParentNavigation { get; set; } = new List<Container>();
}
