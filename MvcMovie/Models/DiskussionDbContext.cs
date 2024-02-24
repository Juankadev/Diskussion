using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Diskussion.Models;

public partial class DiskussionDbContext : DbContext
{
    public DiskussionDbContext()
    {
    }

    public DiskussionDbContext(DbContextOptions<DiskussionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discussion> Discussions { get; set; }

    public virtual DbSet<Response> Responses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discussion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discussi__3214EC07EC3CE518");

            entity.ToTable("Discussion");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.State).HasDefaultValueSql("((1))");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAuthorNavigation).WithMany(p => p.Discussions)
                .HasForeignKey(d => d.IdAuthor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Discussio__IdAut__35BCFE0A");
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Response__3214EC075959F988");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.State).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.IdAuthorNavigation).WithMany(p => p.Responses)
                .HasForeignKey(d => d.IdAuthor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Responses__IdAut__3B75D760");

            entity.HasOne(d => d.IdDiscussionNavigation).WithMany(p => p.Responses)
                .HasForeignKey(d => d.IdDiscussion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Responses__IdDis__3C69FB99");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07780528A5");

            entity.HasIndex(e => e.Name, "UQ_Users_Name").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534E492123D").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Registration_Date");
            entity.Property(e => e.State).HasDefaultValueSql("((1))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
