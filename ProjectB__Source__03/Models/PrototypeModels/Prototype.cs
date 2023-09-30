using System;
using System.Collections.Generic;
using ProjectB.Models.PrototypeVersionModels;

namespace ProjectB.Models.PrototypeModels;

public partial class Prototype
{
    public Guid IdPrototype { get; set; }

    public DateTime? Date { get; set; }

    public TimeSpan? Time { get; set; }

    public string? Title { get; set; }

    public string? ShortId { get; set; }

    public virtual ICollection<PrototypesVersion> PrototypesVersions { get; set; } = new List<PrototypesVersion>();
}
