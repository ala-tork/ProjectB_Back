using ProjectB.Models.PrototypeVersionModels;
using StageTest.Models.ContainerModels;
using StageTest.Models.ContainersVariablesModels;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StageTest.Models.FolderModels;

public partial class ContainersFolder
{
    public Guid IdContainerFolder { get; set; }

    public Guid? IdPrototypeVersion { get; set; }

    public Guid? IdParent { get; set; }

    public Guid? IdFolder { get; set; }

    public string? Title { get; set; }

    public string? TitleDynamic { get; set; }

    public bool? IsDynamicFolderName { get; set; }

    public virtual ICollection<Container> Containers { get; set; } = new List<Container>();

    public virtual ICollection<ContainersVariable> ContainersVariables { get; set; } = new List<ContainersVariable>();

    public virtual ContainersFolder? IdParentNavigation { get; set; }

    public virtual PrototypesVersion? IdPrototypeVersionNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<ContainersFolder> InverseIdParentNavigation { get; set; } = new List<ContainersFolder>();
}
