# Convenciones del Proyecto MesaSitec

Este documento reúne las reglas de negocio, estándares de arquitectura, formato de API y restricciones que deben aplicarse estrictamente en todo el desarrollo.

---

## 1. Reglas Principales de Negocio

### RN-01 — Aislamiento entre organizaciones (Multi-tenancy)
- **Aislamiento absoluto**: Todo acceso a datos se filtra obligatoriamente por el `tenantId` contenido en las claims del token JWT de la sesión.
- **Respuesta 404 en violaciones de tenant**: Si un usuario intenta acceder o modificar un recurso perteneciente a otro tenant, la API DEBE responder con **404 Not Found** (`codigo: "RECURSO_NO_ENCONTRADO"`), **NUNCA 403 Forbidden**, para no revelar la existencia del recurso.

### RN-02 — Máquina de Estados de Solicitudes
Las solicitudes únicamente pueden transicionar según la siguiente tabla:
- **Nueva** $\rightarrow$ `asignar` (a Asignada) | `cancelar` (a Cancelada)
- **Asignada** $\rightarrow$ `iniciar` (a EnProceso) | `asignar` (reasignar) | `cancelar` (a Cancelada)
- **EnProceso** $\rightarrow$ `resolver` (a Resuelta) | `asignar` (a Asignada) | `cancelar` (a Cancelada)
- **Resuelta** $\rightarrow$ `cerrar` (a Cerrada) | `reabrir` (a EnProceso)
- **Cerrada** $\rightarrow$ *(Estado final, sin acciones)*
- **Cancelada** $\rightarrow$ *(Estado final, sin acciones)*

*Cualquier acción no válida responderá **409 Conflict** con `codigo: "TRANSICION_INVALIDA"`.*

### RN-03 — Matriz de Permisos por Rol
Roles existentes: `Admin`, `Agente`, `Solicitante`.
- **Listar todas las solicitudes del tenant**: `Admin` (Sí), `Agente` (Sí), `Solicitante` (No - solo las propias).
- **Ver detalle**: `Admin` (Sí), `Agente` (Sí), `Solicitante` (Sí - solo propias).
- **Crear solicitud**: `Admin` (Sí), `Agente` (Sí), `Solicitante` (Sí).
- **Editar (título, descripción, categoría, prioridad)**: `Admin` (Sí), `Agente` (Sí), `Solicitante` (Sí - solo propias y solo en estado `Nueva`).
- **Acciones `asignar` / `iniciar` / `resolver` / `reabrir`**: `Admin` (Sí), `Agente` (Sí), `Solicitante` (No).
- **Acción `cerrar`**: `Admin` (Sí), `Agente` (Sí), `Solicitante` (Sí - solo propias).
- **Acción `cancelar`**: `Admin` (Sí), `Agente` (No), `Solicitante` (No).

*Cualquier intento no permitido responderá **403 Forbidden** con `codigo: "OPERACION_NO_PERMITIDA"`.*

### RN-04 — Cálculo de SLA
- **Fórmula**: `fechaLimiteSla = fechaCreacion + (categoria.slaHoras * factor[prioridad])`
- **Factores**:
  - `Critica`: $0.5$
  - `Alta`: $0.75$
  - `Media`: $1.0$
  - `Baja`: $2.0$
- **Reglas**:
  - El SLA se calcula **siempre en el servidor**. Si el cliente envía `fechaLimiteSla`, se ignora en silencio.
  - Al cambiar categoría o prioridad en una solicitud no resuelta/cerrada/cancelada, el SLA **se recalcula** (sin modificar `fechaCreacion`).
  - Una solicitud está **vencida** si `fechaLimiteSla < DateTime.UtcNow` y su estado no es `Resuelta`, `Cerrada` ni `Cancelada`.

### RN-05 — Validaciones de Asignación
Al asignar un agente (`agenteId`), debe verificarse que:
1. Existe.
2. Está activo (`activo == true`).
3. Pertenece al mismo `tenantId`.
4. Tiene rol `Agente` o `Admin`.

*Si falla cualquiera, responder **422 Unprocessable Entity** con `codigo: "AGENTE_INVALIDO"`.*

### RN-06 — Justificación de Cierre y Cancelación
- `resolver`: Exige `motivo` de al menos 20 caracteres (`motivoResolucion` y se guarda `fechaResolucion`).
- `cancelar`: Exige `motivo` de al menos 10 caracteres (`motivoCancelacion`).

*Si no se cumple, responder **422 Unprocessable Entity** con `codigo: "MOTIVO_REQUERIDO"`.*

### RN-07 — Formato de Código Correlativo
- Formato: `SOL-{año}-{correlativo 5 dígitos}` (ej. `SOL-2026-00001`).
- El correlativo es independiente por tenant y por año.

---

## 2. Convenciones del Backend (.NET 8 + EF Core)

- **Organización en Capas**:
  - `Dominio`: Entidades, enums, máquina de estados, cálculo SLA, especificaciones de permisos.
  - `Aplicacion`: Servicios, DTOs, casos de uso, interfaces.
  - `Infraestructura`: EF Core `DbContext`, repositorios, generador de JWT, hashing de contraseñas, Seed Data.
  - `Api`: Controllers o Minimal API, Middlewares/ExceptionHandler, configuración Swagger, CORS.
- **Manejo Global de Errores**:
  - Formato `application/problem+json` con campo `codigo` obligatorio.
  - No exponer excepciones no controladas ni stack traces en respuestas HTTP.
- **Autenticación JWT**:
  - HS256, expiración 8 horas.
  - Claims obligatorios: `sub`, `tenantId`, `rol`, `email`.
- **Base de Datos**:
  - SQLite local. Migraciones aplicadas automáticamente al iniciar la aplicación.
  - Datos semilla automáticos respetando `SEED_FECHA_BASE` (`2026-01-15T08:00:00Z`).

---

## 3. Convenciones del Frontend (Vue 3 + TypeScript + Pinia + Vite)

- **TypeScript**:
  - Modo `strict` activado (`tsc --noEmit` en verde).
  - Prohibido el uso de `any` explícito (salvo caso de fuerza mayor debidamente justificado).
  - DTOs de API fuertemente tipados.
- **Componentes**:
  - Vue 3 con `<script setup>`.
  - Manejo obligatorio de 3 estados en vistas/tablas: **cargando**, **vacío** y **error**.
  - Atributos `data-testid` estrictamente respetados según el contrato (sección 7.4 del enunciado).
- **Visibilidad de Botones de Acción**:
  - Los botones `btn-accion-*` que no correspondan al estado actual o al rol del usuario **no deben existir en el DOM** (usar `v-if`, no `v-show` ni `disabled`).
- **Mapeo Cliente HTTP**:
  - Cliente único centralizado que inyecta automáticamente el token JWT y redirige a `/login` ante 401.

---

## 4. Reglas de Operación y Memoria del Agente AI

1. **Lectura obligatoria al iniciar sesión**: Leer siempre `CONTEXTO_SESION.md` y `CONVENCIONES.md`.
2. **Frases disparadoras configuradas**:
   - Registrar en bitácora (`Documentacion/Logs/YYYY-MM-DD.md`): `"vamos a continuar con: <modulo>"`
   - Guardar resumen de sesión (`Documentacion/ResumenesSesion/YYYY-MM-DD-SesionN.md`): `"guarda resumen de la sesion actual"`
3. **Manejo de Suposiciones**: Si hay datos o comportamientos no verificados en el código real, declararlos explícitamente y solicitar confirmación antes de actuar.
4. **Sugerencias de Mejora**: Distinguir la tarea solicitada de sugerencias adicionales. Explicar el motivo técnico concreto y pedir permiso explícito antes de aplicarlas.
