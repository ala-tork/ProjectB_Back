using System;
using System.Collections.Generic;
using ProjectB__Target__01.Models.ProjectsFileModels;

namespace ProjectB__Target__01.Models.ProjectFileLinesModels;

public partial class ProjectsFilesLine
{
    public Guid IdProjectFileLine { get; set; }

    public Guid? IdProjectFile { get; set; }

    public int? LineNumber { get; set; }

    public string? Code { get; set; }

    public bool? IsCrypted { get; set; }

    public virtual ProjectsFile? IdProjectFileNavigation { get; set; }
}
