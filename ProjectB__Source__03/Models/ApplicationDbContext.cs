using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models.PrototypeModels;
using ProjectB.Models.PrototypeVersionModels;
using StageTest.Models.ContainerLineFolder;
using StageTest.Models.ContainerModels;
using StageTest.Models.ContainersVariablesModels;
using StageTest.Models.FolderModels;

namespace StageTest.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Container> Containers { get; set; }

    public virtual DbSet<ContainersFolder> ContainersFolders { get; set; }

    public virtual DbSet<ContainersLine> ContainersLines { get; set; }

    public virtual DbSet<ContainersVariable> ContainersVariables { get; set; }

    public virtual DbSet<ContainersVariablesType> ContainersVariablesTypes { get; set; }

    public virtual DbSet<Prototype> Prototypes { get; set; }

    public virtual DbSet<PrototypesVersion> PrototypesVersions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Container>(entity =>
        {
            entity.HasKey(e => e.IdContainer).HasName("PK_Prototype__Containers");

            entity.Property(e => e.IdContainer).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Extension).HasMaxLength(50);
            entity.Property(e => e.IsDynamicContent).HasColumnName("isDynamicContent");
            entity.Property(e => e.IsDynamicFileName).HasColumnName("isDynamicFileName");
            entity.Property(e => e.TitleDynamic).HasColumnName("Title__Dynamic");

            entity.HasOne(d => d.IdContainerFolderNavigation).WithMany(p => p.Containers)
                .HasForeignKey(d => d.IdContainerFolder)
                .HasConstraintName("FK_Prototype__Containers_Prototype__Containers__Folders");

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_Prototype__Containers_Prototype__Containers");
        });

        modelBuilder.Entity<ContainersFolder>(entity =>
        {
            entity.HasKey(e => e.IdContainerFolder).HasName("PK_Prototype__Containers__Folders");

            entity.ToTable("Containers__Folders");

            entity.Property(e => e.IdContainerFolder).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsDynamicFolderName).HasColumnName("isDynamicFolderName");
            entity.Property(e => e.TitleDynamic).HasColumnName("Title__Dynamic");

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_Containers__Folders_Containers__Folders");

            entity.HasOne(d => d.IdPrototypeVersionNavigation).WithMany(p => p.ContainersFolders)
                .HasForeignKey(d => d.IdPrototypeVersion)
                .HasConstraintName("FK_Containers__Folders_Prototypes__Versions");
        });

        modelBuilder.Entity<ContainersLine>(entity =>
        {
            entity.HasKey(e => e.IdContainerLine).HasName("PK_Prototype__Containers__Lines");

            entity.ToTable("Containers__Lines");

            entity.Property(e => e.IdContainerLine).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdContainerNavigation).WithMany(p => p.ContainersLines)
                .HasForeignKey(d => d.IdContainer)
                .HasConstraintName("FK_Prototype__Containers__Lines_Prototype__Containers");
        });

        modelBuilder.Entity<ContainersVariable>(entity =>
        {
            entity.HasKey(e => e.IdVariable);

            entity.ToTable("Containers__Variables");

            entity.Property(e => e.IdVariable).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TitleDynamic).HasColumnName("Title__Dynamic");

            entity.HasOne(d => d.IdContainerNavigation).WithMany(p => p.ContainersVariables)
                .HasForeignKey(d => d.IdContainer)
                .HasConstraintName("FK_Containers__Variables_Containers");

            entity.HasOne(d => d.IdContainerFolderNavigation).WithMany(p => p.ContainersVariables)
                .HasForeignKey(d => d.IdContainerFolder)
                .HasConstraintName("FK_Containers__Variables_Containers__Folders");

            entity.HasOne(d => d.IdContainerLineNavigation).WithMany(p => p.ContainersVariables)
                .HasForeignKey(d => d.IdContainerLine)
                .HasConstraintName("FK_Containers__Variables_Containers__Lines");

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_Containers__Variables_Containers__Variables");

            entity.HasOne(d => d.IdVariableTypeNavigation).WithMany(p => p.ContainersVariables)
                .HasForeignKey(d => d.IdVariableType)
                .HasConstraintName("FK_Containers__Variables_Containers__Variables__Types");
        });

        modelBuilder.Entity<ContainersVariablesType>(entity =>
        {
            entity.HasKey(e => e.IdVariableType);

            entity.ToTable("Containers__Variables__Types");

            entity.Property(e => e.IdVariableType).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_Containers__Variables__Types_Containers__Variables__Types");
        });

        modelBuilder.Entity<Prototype>(entity =>
        {
            entity.HasKey(e => e.IdPrototype).HasName("PK_Prototype");

            entity.Property(e => e.IdPrototype).HasDefaultValueSql("(newid())");
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
        });

        modelBuilder.Entity<PrototypesVersion>(entity =>
        {
            entity.HasKey(e => e.IdPrototypeVersion).HasName("PK_Prototype__Versions");

            entity.ToTable("Prototypes__Versions");

            entity.Property(e => e.IdPrototypeVersion).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("Date_");
            entity.Property(e => e.IsBackend).HasColumnName("isBackend");
            entity.Property(e => e.IsFrontend).HasColumnName("isFrontend");
            entity.Property(e => e.IsLastVersion).HasColumnName("isLastVersion");
            entity.Property(e => e.ShortId)
                .HasMaxLength(10)
                .HasColumnName("ShortID");
            entity.Property(e => e.Time).HasPrecision(0);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.IdPrototypeNavigation).WithMany(p => p.PrototypesVersions)
                .HasForeignKey(d => d.IdPrototype)
                .HasConstraintName("FK_Prototype__Versions_Prototype");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
