# Módulo: Backend Infraestructura y Base de Datos

## Estado Actual
Pendiente

## Historial de Cambios y Decisiones
- **2026-07-31**: Creación del registro del módulo.

## Alcance del Módulo
- EF Core `DbContext` (`MesaSitecDbContext`).
- Configuración de entidades con `IEntityTypeConfiguration` (índices, claves foráneas, restricciones de longitud).
- Migraciones automáticas SQLite aplicadas al iniciar la API.
- Hashing seguro de contraseñas mediante BCrypt / `PasswordHasher`.
- Generación y Validación de Tokens JWT (HS256, 8h expiración).
- Carga de Datos Semilla (Seed Data) respetando `SEED_FECHA_BASE` (default `2026-01-15T08:00:00Z`).

## Pendientes
- [ ] Configurar DbContext con SQLite.
- [ ] Implementar el servicio Seeder con desplazamientos fijos a partir de `SEED_FECHA_BASE`.
