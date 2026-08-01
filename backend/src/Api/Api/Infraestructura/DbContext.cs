using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Infraestructura;

public class MesaSitecDbContext : DbContext
{
    public  MesaSitecDbContext(DbContextOptions<MesaSitecDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Solicitud> Solicitudes => Set<Solicitud>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

  
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(t => t.Activo).IsRequired();

            entity.HasMany(t => t.Usuarios)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(t => t.Categorias)
                .WithOne(c => c.Tenant)
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(t => t.Solicitudes)
                .WithOne(s => s.Tenant)
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

       
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            entity.Property(u => u.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(u => u.Rol).IsRequired().HasConversion<string>();
            entity.Property(u => u.Activo).IsRequired();
        });

    
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(c => c.SlaHoras).IsRequired();
            entity.Property(c => c.Activo).IsRequired();

            entity.HasMany(c => c.Solicitudes)
                .WithOne(s => s.Categoria)
                .HasForeignKey(s => s.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

       
        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Codigo).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Titulo).IsRequired().HasMaxLength(120);
            entity.Property(s => s.Descripcion).IsRequired().HasMaxLength(4000);
            entity.Property(s => s.Prioridad).IsRequired().HasConversion<string>();
            entity.Property(s => s.Estado).IsRequired().HasConversion<string>();
            entity.Property(s => s.FechaCreacion).IsRequired();
            entity.Property(s => s.FechaLimiteSla).IsRequired();
            entity.Property(s => s.MotivoResolucion).HasMaxLength(2000);
            entity.Property(s => s.MotivoCancelacion).HasMaxLength(2000);

          
            entity.HasOne(s => s.Solicitante)
                .WithMany(u => u.SolicitudesCreadas)
                .HasForeignKey(s => s.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

           
            entity.HasOne(s => s.Agente)
                .WithMany(u => u.SolicitudesAsignadas)
                .HasForeignKey(s => s.AgenteId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
