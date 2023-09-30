using System;
using System.Collections.Generic;
using ProjectB__Target__01.Models.ProjectModels;
using ProjectB__Target__01.Models.ProjectsFileModels;
using ProjectB__Target__01.Models.ProjectsFolderModels;

namespace ProjectB__Target__01.Models.ProjectVersionModels;

public partial class ProjectsVersion
{
    public Guid IdProjectVersion { get; set; }

    public Guid? IdProject { get; set; }

    public Guid? IdParent { get; set; }

    public Guid? PrototypeEditorPrototypesVersionsIdPrototypeVersion { get; set; }

    public string? Title { get; set; }

    public int? Version { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsFrontend { get; set; }

    public bool? IsBackend { get; set; }

    public bool? IsLastVersion { get; set; }

    public string? Time { get; set; }

    public string? ShortId { get; set; }

    public string? Path { get; set; }

    public bool? IsCrypted { get; set; }

    public virtual ProjectsVersion? IdParentNavigation { get; set; }

    public virtual Project? IdProjectNavigation { get; set; }

    public virtual ICollection<ProjectsVersion> InverseIdParentNavigation { get; set; } = new List<ProjectsVersion>();

    public virtual ICollection<ProjectsFile> ProjectsFiles { get; set; } = new List<ProjectsFile>();

    public virtual ICollection<ProjectsFolder> ProjectsFolders { get; set; } = new List<ProjectsFolder>();
}
