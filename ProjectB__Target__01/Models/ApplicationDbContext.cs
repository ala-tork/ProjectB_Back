using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectB__Target__01.Models.ProjectFileLinesModels;
using ProjectB__Target__01.Models.ProjectModels;
using ProjectB__Target__01.Models.ProjectsFileModels;
using ProjectB__Target__01.Models.ProjectsFolderModels;
using ProjectB__Target__01.Models.ProjectVersionModels;

namespace ProjectB__Target__01.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectsFile> ProjectsFiles { get; set; }

    public virtual DbSet<ProjectsFilesLine> ProjectsFilesLines { get; set; }

    public virtual DbSet<ProjectsFolder> ProjectsFolders { get; set; }

    public virtual DbSet<ProjectsVersion> ProjectsVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.IdProject);

            entity.Property(e => e.IdProject).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("Date_");
            entity.Property(e => e.ShortId)
                .HasMaxLength(10)
                .HasColumnName("ShortID");
            entity.Property(e => e.Time)
                .HasPrecision(0)
                .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Version).HasMaxLength(50);
        });

        modelBuilder.Entity<ProjectsFile>(entity =>
        {
            entity.HasKey(e => e.IdProjectFile);

            entity.ToTable("Projects__Files");

            entity.Property(e => e.IdProjectFile).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Extension).HasMaxLength(50);
            entity.Property(e => e.FileCreationDate)
                .HasColumnType("date")
                .HasColumnName("File__CreationDate");
            entity.Property(e => e.FileCreationTime)
                .HasPrecision(0)
                .HasColumnName("File__CreationTime");
            entity.Property(e => e.FileLastUpdateDate)
                .HasColumnType("date")
                .HasColumnName("File__LastUpdateDate");
            entity.Property(e => e.FileLastUpdateTime)
                .HasPrecision(0)
                .HasColumnName("File__LastUpdateTime");
            entity.Property(e => e.FileSize).HasColumnName("File__Size");

            entity.HasOne(d => d.IdProjectFolderNavigation).WithMany(p => p.ProjectsFiles)
                .HasForeignKey(d => d.IdProjectFolder)
                .HasConstraintName("FK_Projects__Files_Projects__Folders");

            entity.HasOne(d => d.IdProjectVersionNavigation).WithMany(p => p.ProjectsFiles)
                .HasForeignKey(d => d.IdProjectVersion)
                .HasConstraintName("FK_Projects__Files_Projects__Versions");
        });

        modelBuilder.Entity<ProjectsFilesLine>(entity =>
        {
            entity.HasKey(e => e.IdProjectFileLine);

            entity.ToTable("Projects__Files__Lines");

            entity.Property(e => e.IdProjectFileLine).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsCrypted).HasColumnName("isCrypted");

            entity.HasOne(d => d.IdProjectFileNavigation).WithMany(p => p.ProjectsFilesLines)
                .HasForeignKey(d => d.IdProjectFile)
                .HasConstraintName("FK_Projects__Files__Lines_Projects__Files");
        });

        modelBuilder.Entity<ProjectsFolder>(entity =>
        {
            entity.HasKey(e => e.IdProjectFolder);

            entity.ToTable("Projects__Folders");

            entity.Property(e => e.IdProjectFolder).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_Projects__Folders_Projects__Folders");

            entity.HasOne(d => d.IdProjectVersionNavigation).WithMany(p => p.ProjectsFolders)
                .HasForeignKey(d => d.IdProjectVersion)
                .HasConstraintName("FK_Projects__Folders_Projects__Versions");
        });

        modelBuilder.Entity<ProjectsVersion>(entity =>
        {
            entity.HasKey(e => e.IdProjectVersion);

            entity.ToTable("Projects__Versions");

            entity.Property(e => e.IdProjectVersion).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("Date_");
            entity.Property(e => e.IsBackend).HasColumnName("isBackend");
            entity.Property(e => e.IsCrypted).HasColumnName("isCrypted");
            entity.Property(e => e.IsFrontend).HasColumnName("isFrontend");
            entity.Property(e => e.IsLastVersion).HasColumnName("isLastVersion");
            entity.Property(e => e.PrototypeEditorPrototypesVersionsIdPrototypeVersion).HasColumnName("Prototype_Editor.Prototypes__Versions.IdPrototypeVersion");
            entity.Property(e => e.ShortId)
                .HasMaxLength(10)
                .HasColumnName("ShortID");
            entity.Property(e => e.Time).HasMaxLength(10);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_Projects__Versions_Projects__Versions");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.ProjectsVersions)
                .HasForeignKey(d => d.IdProject)
                .HasConstraintName("FK_Projects__Versions_Projects");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
