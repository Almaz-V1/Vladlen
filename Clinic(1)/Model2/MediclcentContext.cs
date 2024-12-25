using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Clinic_1_.Model2;

public partial class MediclcentContext : DbContext
{
    public MediclcentContext()
    {
    }

    public MediclcentContext(DbContextOptions<MediclcentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Chek> Cheks { get; set; }

    public virtual DbSet<Emplo> Emploes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Specialist> Specialists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=12345;database=mediclcent", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.IdAppointment).HasName("PRIMARY");

            entity.ToTable("appointment");

            entity.HasIndex(e => e.IdCart, "idCart_idx");

            entity.Property(e => e.IdAppointment).HasColumnName("idAppointment");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.LasteNames).HasMaxLength(45);
            entity.Property(e => e.Names).HasMaxLength(45);
            entity.Property(e => e.Patronomicl).HasMaxLength(45);
            entity.Property(e => e.Spec).HasMaxLength(45);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.IdCart).HasName("PRIMARY");

            entity.ToTable("cart");

            entity.HasIndex(e => e.IdEmpoes, "Emplo_idx");

            entity.Property(e => e.IdCart).HasColumnName("idCart");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.IdEmpoes).HasColumnName("idEmpoes");
            entity.Property(e => e.LasteName).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Patronomick).HasMaxLength(45);
            entity.Property(e => e.Spec).HasMaxLength(45);

            entity.HasOne(d => d.IdEmpoesNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.IdEmpoes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Emplo");
        });

        modelBuilder.Entity<Chek>(entity =>
        {
            entity.HasKey(e => e.Idchek).HasName("PRIMARY");

            entity.ToTable("chek");

            entity.HasIndex(e => e.IdCart, "cart_idx");

            entity.Property(e => e.Idchek).HasColumnName("idchek");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IdCart).HasColumnName("idCart");
            entity.Property(e => e.Lastename).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Patronomick).HasMaxLength(45);
            entity.Property(e => e.Profecsens).HasMaxLength(45);

            entity.HasOne(d => d.IdCartNavigation).WithMany(p => p.Cheks)
                .HasForeignKey(d => d.IdCart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart");
        });

        modelBuilder.Entity<Emplo>(entity =>
        {
            entity.HasKey(e => e.IdDoctors).HasName("PRIMARY");

            entity.ToTable("emploes");

            entity.HasIndex(e => e.IdProfeshens, "Profeshens_idx");

            entity.HasIndex(e => e.NameIdRoles, "id_roles_idx");

            entity.Property(e => e.IdDoctors).HasColumnName("idDoctors");
            entity.Property(e => e.CabinetNumber).HasMaxLength(6);
            entity.Property(e => e.Emaile).HasMaxLength(25);
            entity.Property(e => e.IdProfeshens).HasColumnName("idProfeshens");
            entity.Property(e => e.LasteName).HasMaxLength(20);
            entity.Property(e => e.MiddleName).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.NomerPasport)
                .HasMaxLength(6)
                .HasColumnName("nomerPasport");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasMaxLength(12);
            entity.Property(e => e.SeriaPasport)
                .HasMaxLength(4)
                .HasColumnName("seriaPasport");

            entity.HasOne(d => d.IdProfeshensNavigation).WithMany(p => p.Emplos)
                .HasForeignKey(d => d.IdProfeshens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("worker");

            entity.HasOne(d => d.NameIdRolesNavigation).WithMany(p => p.Emplos)
                .HasForeignKey(d => d.NameIdRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ole");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRoles).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.IdRoles).HasColumnName("idRoles");
            entity.Property(e => e.Title).HasMaxLength(25);
        });

        modelBuilder.Entity<Specialist>(entity =>
        {
            entity.HasKey(e => e.IdSpecialist).HasName("PRIMARY");

            entity.ToTable("specialist");

            entity.Property(e => e.IdSpecialist).HasColumnName("idSpecialist");
            entity.Property(e => e.TitleSpecialist).HasMaxLength(25);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsers).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.IdRole, "roles_idx");

            entity.Property(e => e.IdUsers).HasColumnName("idUsers");
            entity.Property(e => e.Firstname)
                .HasMaxLength(20)
                .HasColumnName("firstname");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.Lastname)
                .HasMaxLength(20)
                .HasColumnName("lastname");
            entity.Property(e => e.Login)
                .HasMaxLength(25)
                .HasColumnName("login");
            entity.Property(e => e.Partominyc)
                .HasMaxLength(20)
                .HasColumnName("partominyc");
            entity.Property(e => e.Password)
                .HasMaxLength(6)
                .HasColumnName("password");
            entity.Property(e => e.PasswordNumber)
                .HasMaxLength(25)
                .HasColumnName("password_number");
            entity.Property(e => e.PasswordSeries)
                .HasMaxLength(4)
                .HasColumnName("password_series");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
