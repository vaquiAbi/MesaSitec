# Módulo: Backend API y Endpoints

## Estado Actual
Pendiente

## Historial de Cambios y Decisiones
- **2026-07-31**: Creación del registro del módulo.

## Alcance del Módulo
- Endpoints REST bajo `/api/v1`:
  1. `POST /auth/login`
  2. `GET /me`
  3. `GET /categorias`
  4. `GET /solicitudes`
  5. `POST /solicitudes`
  6. `GET /solicitudes/{id}`
  7. `PUT /solicitudes/{id}`
  8. `POST /solicitudes/{id}/transiciones`
  9. `GET /health` (sin autenticación)
- Middleware/Handler de Excepciones Global (`ProblemDetails` con `codigo`).
- Configuración de Swagger `/swagger` con autenticación Bearer JWT.
- Configuración de CORS para `http://localhost:5173`.

## Pendientes
- [ ] Implementar la captura global de excepciones mapeadas a los códigos HTTP y `codigo` estándar.
- [ ] Configurar el endpoint `/health` y documentación Swagger.
