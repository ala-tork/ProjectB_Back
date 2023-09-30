using System;
using System.Collections.Generic;
using ProjectB__Target__01.Models.ProjectFileLinesModels;
using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Models.ProjectVersionModels;

namespace ProjectB__Target__01.Models.ProjectsFileModels;

public partial class ProjectsFile
{
    public Guid IdProjectFile { get; set; }

    public Guid? IdProjectVersion { get; set; }

    public Guid? IdProjectFolder { get; set; }

    public string? Title { get; set; }

    public string? Extension { get; set; }

    public DateTime? FileCreationDate { get; set; }

    public TimeSpan? FileCreationTime { get; set; }

    public DateTime? FileLastUpdateDate { get; set; }

    public TimeSpan? FileLastUpdateTime { get; set; }

    public int? FileSize { get; set; }

    public virtual ProjectsFolder? IdProjectFolderNavigation { get; set; }

    public virtual ProjectsVersion? IdProjectVersionNavigation { get; set; }

    public virtual ICollection<ProjectsFilesLine> ProjectsFilesLines { get; set; } = new List<ProjectsFilesLine>();
}
