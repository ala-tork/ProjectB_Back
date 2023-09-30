using System;
using System.Collections.Generic;
using ProjectB__Target__01.Models.ProjectVersionModels;

namespace ProjectB__Target__01.Models.ProjectModels;

public partial class Project
{
    public Guid IdProject { get; set; }

    public string? Title { get; set; }

    public string? Version { get; set; }

    public DateTime? Date { get; set; }

    public TimeSpan? Time { get; set; }

    public string? ShortId { get; set; }

    public virtual ICollection<ProjectsVersion> ProjectsVersions { get; set; } = new List<ProjectsVersion>();
}
