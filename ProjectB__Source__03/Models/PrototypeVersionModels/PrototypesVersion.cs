using System;
using System.Collections.Generic;
using ProjectB.Models.PrototypeModels;
using StageTest.Models.FolderModels;

namespace ProjectB.Models.PrototypeVersionModels;

public partial class PrototypesVersion
{
    public Guid IdPrototypeVersion { get; set; }

    public Guid? IdPrototype { get; set; }

    public string? Title { get; set; }

    public int? Version { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsFrontend { get; set; }

    public bool? IsBackend { get; set; }

    public bool? IsLastVersion { get; set; }

    public TimeSpan? Time { get; set; }

    public string? ShortId { get; set; }

    public string? Path { get; set; }

    public virtual ICollection<ContainersFolder> ContainersFolders { get; set; } = new List<ContainersFolder>();

    public virtual Prototype? IdPrototypeNavigation { get; set; }
}
