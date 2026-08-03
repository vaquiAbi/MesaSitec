
##  Requisitos Previos



- **.NET 8.0 SDK**



##  Cómo Levantar el Proyecto (Paso a Paso)

Abrir un terminal : Win+R -> escribir cmd  -> enter
En la terminal escribir  los siguientes comandos(después de cada comando presionar enter)
1.  md MesaSitec_Abigail && cd MesaSitec_Abigail
2.  git clone https://github.com/vaquiAbi/MesaSitec.git
3.  dotnet run --project MesaSitec/backend/src/Api/Api/Api.csproj --launch-profile "http"
Ir al enlace indicado abajo:
4. http://localhost:5080/swagger




## Credenciales de Prueba (Datos Semilla)

Todos los usuarios tienen la contraseña por defecto: `Sitec.2026`

### Organización 1: Cooperativa Norte
| Email | Rol |
|---|---|
| `admin@norte.test` | Admin |
| `agente1@norte.test` | Agente |
| `agente2@norte.test` | Agente |
| `user1@norte.test` | Solicitante |
| `user2@norte.test` | Solicitante |

### Organización 2: Bufete Sur
| Email | Rol |
|---|---|
| `admin@sur.test` | Admin |
| `user1@sur.test` | Solicitante |

---

##  Estado de Implementación

### Backend (.NET 8 + EF Core)
- [x] **Aislamiento entre organizaciones**
- [x] **Autenticación JWT**
- [x] **Manejador Global de Errores**
- [x] **Base de Datos y Semilla**
- [x] **`GET /health`**
- [x] **`POST /api/v1/auth/login`**
- [x] **`GET /api/v1/me`**
- [x] **`GET /api/v1/solicitudes`**
- [ ] **`GET /api/v1/categorias`**: *(Pendiente de implementación)*.
- [ ] **`POST /api/v1/solicitudes`**:  *(Pendiente de implementación)*.
- [ ] **`GET /api/v1/solicitudes/{id}`**:  *(Pendiente de implementación)*.
- [ ] **`PUT /api/v1/solicitudes/{id}`**:  *(Pendiente de implementación)*..
- [ ] **`POST /api/v1/solicitudes/{id}/transiciones`**:  *(Pendiente de implementación)*.
- [ ] **Pruebas Unitarias xUnit**:  *(Pendiente de implementación)*.
- [ ] **Máquina de estados**:  *(Pendiente de implementación)*.


### Frontend 
- Esta parte no se desarrollo.

