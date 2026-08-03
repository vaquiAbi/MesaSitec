


### A. Centralización de claims mediante `BaseApiController`
- **Alternativa descartada**: Extraer los claims manualmente en cada método de controlador. Opción descartada ya que al tener un contralador dedicado a la extracción se evita código repetido, y tambien un estándar en errores cuando el claim sea nulo o invalido.


### B. Uso de DTOs (Request / Response) para contratos de petición y respuesta
- **Alternativa descartada**: Retornar o recibir las entidades de EF Core (`Solicitud`, `Usuario`, etc.) directamente en los controladores de la API. Ya que se necesita diferente información en las peticiones y en las respuestas, además que existen datos que no deben ser accesibles o exponer el esquema de la base de datos.


### C. Manejo global de errores por medio de `MiddlewareErrores`
- **Alternativa descartada**: Manejar bloques `try-catch` individuales en cada endpoint. En cada petición tener un bloque try-catch, queda la posibilidad de no capturar una excepción  y generar un error sin documentar, al obtener la respuesta antes de ser envíada por medio del MiddlewareErrores podemos verificar el error  y asi poder documentarlo.

---

## 2. Declaración del uso de IA

- **Desarrollado con asistencia de IA**:
  - Generación del código.
  - Generación de datos semilla estáticos en `DbSembrar.cs`.
- **Desarrollado a mano y supervisado directamente**:
  - Diseño de las reglas del proyecto.
  - Estructura y flujo de controladores realizados.


---

## 3. Lo que haría distinto con una semana más

1. **Pruebas unitarias de la mano con el desarrollo**: 
2. **Desarrollo del frontend junto con el desarrollo de cada endpoint**:
---

## 4. Punto de Atasco y Resolución

- **El Problema**:
 - La estructura del proyecto, inicialmente se pensó realizarlo con dll.
- **Análisis y Resolución**:
 - Con el manejo por dll se tenia la opción de un aislamiento total de entidades y servicios, además que se podia compartir el código entre proyectos, sin embargo la implementación resultaba mas compleja. Por lo que se decició realizarlo en un solo proyecto organizado por carpetas, ademas que de esta forma tambien la rapidez de la ejecución es mejor.