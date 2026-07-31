# Módulo: Frontend Vistas de Solicitudes

## Estado Actual
Pendiente

## Historial de Cambios y Decisiones
- **2026-07-31**: Creación del registro del módulo.

## Alcance del Módulo
- Vista Listado `/solicitudes` (tabla, filtros server-side, paginación con formato exacto `Página X de Y — Z resultados`, badges).
- Vista Nueva `/solicitudes/nueva` y Editar `/solicitudes/:id/editar` (componente reutilizable de formulario).
- Vista Detalle `/solicitudes/:id` (detalle completo, modales de acción).
- Regla de Visibilidad DOM: Los botones de acción `btn-accion-*` no permitidos NO deben existir en el DOM.
- Todos los `data-testid` requeridos por la sección 7.4.

## Pendientes
- [ ] Implementar la vista del listado con paginación server-side.
- [ ] Implementar formularios y modal de transiciones de estado con justificación.
