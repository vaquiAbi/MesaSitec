using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Excepciones;
using Api.Modelos.Dto.Response;

namespace Api.Infraestructura;

public class MiddlewareErrores
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MiddlewareErrores> _logger;

    public MiddlewareErrores(RequestDelegate next, ILogger<MiddlewareErrores> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (!context.Response.HasStarted)
            {
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await EscribirRespuestaErrorAsync(context, new ErrorDetalleDto
                    {
                        Type = "https://mesasitec.local/errores/no-autenticado",
                        Title = "No autenticado",
                        Status = StatusCodes.Status401Unauthorized,
                        Detail = "Token ausente, inválido o expirado.",
                        Codigo = "NO_AUTENTICADO"
                    });
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await EscribirRespuestaErrorAsync(context, new ErrorDetalleDto
                    {
                        Type = "https://mesasitec.local/errores/operacion-no-permitida",
                        Title = "Operación no permitida",
                        Status = StatusCodes.Status403Forbidden,
                        Detail = "El rol no permite la operación.",
                        Codigo = "OPERACION_NO_PERMITIDA"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted) return;

            ErrorDetalleDto errorDto;

            if (ex is ExcepcionApi apiEx)
            {
                _logger.LogWarning(apiEx, "Excepción de API interceptada: {Codigo}", apiEx.Codigo);
                errorDto = new ErrorDetalleDto
                {
                    Type = apiEx.Type,
                    Title = apiEx.Title,
                    Status = apiEx.StatusCode,
                    Detail = apiEx.Message,
                    Codigo = apiEx.Codigo,
                    Errores = apiEx.Errores
                };
            }
            else
            {
                _logger.LogError(ex, "Excepción no controlada en MiddlewareErrores");
                errorDto = new ErrorDetalleDto
                {
                    Type = "https://mesasitec.local/errores/error-interno",
                    Title = "Error interno del servidor",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message,
                    Codigo = "ERROR_INTERNO"
                };
            }

            await EscribirRespuestaErrorAsync(context, errorDto);
        }
    }

    private static async Task EscribirRespuestaErrorAsync(HttpContext context, ErrorDetalleDto errorDto)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = errorDto.Status;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(errorDto, options);
        await context.Response.WriteAsync(json);
    }
}
