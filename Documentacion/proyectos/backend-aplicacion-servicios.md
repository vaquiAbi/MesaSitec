# Módulo: Backend Aplicación y Servicios

## Estado Actual
Pendiente

## Historial de Cambios y Decisiones
- **2026-07-31**: Creación del registro del módulo.

## Alcance del Módulo
- DTOs de Solicitud y Respuesta (Auth, Solicitud, Categorías, Usuarios).
- Casos de uso / Servicios de Aplicación:
  - `IAuthService` (login, obtención de perfil).
  - `ICategoriaService` (obtención de categorías activas por tenant).
  - `ISolicitudService` (crear, editar, listar paginado/filtrado en servidor, detalle, transiciones de estado).
- Validación de aislamiento multi-tenant a nivel de caso de uso.

## Pendientes
- [ ] Definir DTOs con nombres exactos en `camelCase` requeridos por el contrato.
- [ ] Implementar la paginación, filtrado, ordenamiento semántico y búsqueda de solicitudes en servidor.
