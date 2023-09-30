using System;
using System.Collections.Generic;
using ProjectB__Target__01.Models.ProjectsFileModels;
using ProjectB__Target__01.Models.ProjectVersionModels;

namespace ProjectB__Target__01.Models.ProjectsFolderModels;

public partial class ProjectsFolder
{
    public Guid IdProjectFolder { get; set; }

    public Guid? IdProjectVersion { get; set; }

    public Guid? IdParent { get; set; }

    public string? Path { get; set; }

    public string? Title { get; set; }

    public virtual ProjectsFolder? IdParentNavigation { get; set; }

    public virtual ProjectsVersion? IdProjectVersionNavigation { get; set; }

    public virtual ICollection<ProjectsFolder> InverseIdParentNavigation { get; set; } = new List<ProjectsFolder>();

    public virtual ICollection<ProjectsFile> ProjectsFiles { get; set; } = new List<ProjectsFile>();
}
