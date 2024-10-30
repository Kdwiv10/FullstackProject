using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FullstackProject.Model;

public partial class S22024Group2ProjectContext : DbContext
{
    public S22024Group2ProjectContext()
    {
    }

    public S22024Group2ProjectContext(DbContextOptions<S22024Group2ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameLibrary> GameLibraries { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //Removed Warning
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=citizen.manukautech.info,6304;Database=S22024_Group2_Project;UID=S22024_Group2;PWD=fBit$95124;encrypt=true;trustservercertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD797A3AEE056");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId)
                .ValueGeneratedNever()
                .HasColumnName("CartID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Carts)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Cart__GameID__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cart__UserID__68487DD7");
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

            entity.Property(e => e.GameId)
                .ValueGeneratedNever()
                .HasColumnName("GameID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.DeveloperId).HasColumnName("DeveloperID");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Developer).WithMany(p => p.Games)
                .HasForeignKey(d => d.DeveloperId)
                .HasConstraintName("FK__Games__Developer__52593CB8");
        });

        modelBuilder.Entity<GameLibrary>(entity =>
        {
            entity.HasKey(e => e.LibraryId).HasName("PK__GameLibr__A13647BF43F008E5");

            entity.ToTable("GameLibrary");

            entity.Property(e => e.LibraryId)
                .ValueGeneratedNever()
                .HasColumnName("LibraryID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.PricePaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.GameLibraries)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__GameLibra__GameI__5629CD9C");

            entity.HasOne(d => d.User).WithMany(p => p.GameLibraries)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__GameLibra__UserI__5535A963");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A785990BEA4EC2");

            entity.HasIndex(e => e.UserId, "UQ__Membersh__1788CCADDB5B030F").IsUnique();

            entity.Property(e => e.MembershipId)
                .ValueGeneratedNever()
                .HasColumnName("MembershipID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Membership)
                .HasForeignKey<Membership>(d => d.UserId)
                .HasConstraintName("FK__Membershi__UserI__59FA5E80");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A5810BBE4EE");

            entity.Property(e => e.PaymentId)
                .ValueGeneratedNever()
                .HasColumnName("PaymentID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");

            entity.HasOne(d => d.Purchase).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK__Payments__Purcha__656C112C");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BDE536E93D6");

            entity.Property(e => e.PurchaseId)
                .ValueGeneratedNever()
                .HasColumnName("PurchaseID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.PricePaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Purchases__GameI__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Purchases__UserI__619B8048");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AEB172D10E");

            entity.Property(e => e.ReviewId)
                .ValueGeneratedNever()
                .HasColumnName("ReviewID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Reviews__GameID__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__UserID__5EBF139D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A139672AF");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.RoleType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC0F3D84C5");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105349FA7B6FF").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProfileInfo).HasColumnType("text");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
