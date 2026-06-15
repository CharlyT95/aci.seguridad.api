using Aduanas.Aci.Seguridad.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Seguridad.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario>            Usuario            => Set<Usuario>();
    public DbSet<UsuarioCredencial>  UsuarioCredencial => Set<UsuarioCredencial>();
    public DbSet<UsuarioRol>         UsuarioRol        => Set<UsuarioRol>();
    public DbSet<Rol>                Rol               => Set<Rol>();
    public DbSet<Permiso>            Permiso            => Set<Permiso>();
    public DbSet<RolPermiso>         RolPermiso         => Set<RolPermiso>();
    public DbSet<RefreshToken>       RefreshToken       => Set<RefreshToken>();
    public DbSet<Parametro>         Parametro           => Set<Parametro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // UsuarioRol
        modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.Usuario)
            .WithMany()
            .HasForeignKey(ur => ur.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.Rol)
            .WithMany()
            .HasForeignKey(ur => ur.IdRol)
            .OnDelete(DeleteBehavior.Restrict);

        // RolPermiso
        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Rol)
            .WithMany()
            .HasForeignKey(rp => rp.IdRol)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Permiso)
            .WithMany()
            .HasForeignKey(rp => rp.IdPermiso)
            .OnDelete(DeleteBehavior.Restrict);

        // UsuarioCredencial
        modelBuilder.Entity<UsuarioCredencial>()
            .HasOne(uc => uc.Usuario)
            .WithMany()
            .HasForeignKey(uc => uc.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // RefreshToken
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.Usuario)
            .WithMany()
            .HasForeignKey(rt => rt.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice en Token para búsquedas rápidas
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(rt => rt.TokenHash)
            .IsUnique();

        modelBuilder.Entity<RefreshToken>()
            .ToTable("RefreshToken")  // ← nombre exacto de la BD
            .HasOne(rt => rt.Usuario)
            .WithMany()
            .HasForeignKey(rt => rt.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasIndex(rt => rt.TokenHash)
            .IsUnique();

        modelBuilder.Entity<Parametro>(entity =>
        {
            entity.ToTable("Parametro");
            entity.HasKey(p => p.IdParametro);
            entity.Property(p => p.CodigoParametro).HasMaxLength(25).IsRequired();
            entity.Property(p => p.Valor).HasMaxLength(150).IsRequired();
            entity.Property(p => p.Descripcion).HasMaxLength(250);
            entity.Property(p => p.UsuarioCreacion).HasMaxLength(50).IsRequired();
            entity.Property(p => p.UsuarioModificacion).HasMaxLength(50);
            entity.Property(p => p.Activo).HasDefaultValue(true);
        });
    }
}
