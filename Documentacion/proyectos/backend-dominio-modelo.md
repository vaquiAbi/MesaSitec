# Módulo: Backend Dominio y Modelo

## Estado Actual
Pendiente

## Historial de Cambios y Decisiones
- **2026-07-31**: Creación del registro del módulo.

## Alcance del Módulo
- Modelado de Entidades del Dominio: `Tenant`, `Usuario`, `Categoria`, `Solicitud`.
- Implementación de Enums: `Rol` (`Admin`, `Agente`, `Solicitante`), `Prioridad` (`Baja`, `Media`, `Alta`, `Critica`), `Estado` (`Nueva`, `Asignada`, `EnProceso`, `Resuelta`, `Cerrada`, `Cancelada`).
- Reglas del Dominio:
  - Máquina de Estados (RN-02)
  - Especificación de Permisos por Rol (RN-03)
  - Servicio / Cálculo de SLA en servidor (RN-04)
  - Validación de Asignaciones (RN-05)
  - Validación de Motivos (RN-06)
  - Generación de Código Correlativo (RN-07)

## Pendientes
- [ ] Separar la solución en proyectos de backend en capas: `Dominio`, `Aplicacion`, `Infraestructura`, `Api`.
- [ ] Implementar la máquina de estados desacoplada para pruebas unitarias sin dependencias externas.
- [ ] Implementar la lógica del cálculo de SLA.
