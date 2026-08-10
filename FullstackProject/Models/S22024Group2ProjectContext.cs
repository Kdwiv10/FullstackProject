using System;
using System.Collections.Generic;
using FullStack_Shangri.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FullStack_Shangri.Models;

public partial class S22024Group2ProjectContext : DbContext
{
    public S22024Group2ProjectContext()
    {
    }

    public S22024Group2ProjectContext(DbContextOptions<S22024Group2ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameLibrary> GameLibraries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Memberships> Memberships { get; set; }

    public virtual DbSet<UserMembershipViewModel> UserMembership { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.DeveloperId).HasName("PK__Develope__DE084CD149B01792");

            entity.HasIndex(e => e.Email, "UQ__Develope__A9D10534268086DC").IsUnique();

            entity.Property(e => e.DeveloperId)
                .ValueGeneratedNever()
                .HasColumnName("DeveloperID");
            entity.Property(e => e.ContactInfo).HasColumnType("text");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Games__2AB897DDDAE95024");

            entity.HasIndex(e => e.DeveloperId, "IX_Games_DeveloperID");

            entity.Property(e => e.GameId)
                .ValueGeneratedNever()
                .HasColumnName("GameID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.DeveloperId).HasColumnName("DeveloperID");
            entity.Property(e => e.ImageFileName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Developer).WithMany(p => p.Games)
                .HasForeignKey(d => d.DeveloperId)
                .HasConstraintName("FK__Games__Developer__52593CB8");

            entity.HasOne(d => d.Genre).WithMany(p => p.Games)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_Games_Genres");
        });

        modelBuilder.Entity<GameLibrary>(entity =>
        {
            entity.HasKey(e => e.LibraryId).HasName("PK__GameLibr__A13647BF43F008E5");

            entity.ToTable("GameLibrary");

            entity.HasIndex(e => e.GameId, "IX_GameLibrary_GameID");

            entity.HasIndex(e => e.UserId, "IX_GameLibrary_UserID");

            entity.Property(e => e.LibraryId)
                .ValueGeneratedNever()
                .HasColumnName("LibraryID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.PricePaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.GameLibraries)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__GameLibra__GameI__5629CD9C");
        });

        modelBuilder.Entity<Memberships>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A785990BEA4EC2");

            entity.Property(e => e.MembershipId)
                .ValueGeneratedNever()
                .HasColumnName("MembershipID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AEB172D10E");

            entity.HasIndex(e => e.GameId, "IX_Reviews_GameID");

            entity.HasIndex(e => e.UserId, "IX_Reviews_UserID");

            entity.Property(e => e.ReviewId)
                .ValueGeneratedNever()
                .HasColumnName("ReviewID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Reviews__GameID__5DCAEF64");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
