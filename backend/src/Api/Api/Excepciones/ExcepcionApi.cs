namespace Api.Excepciones;

public class ExcepcionApi : Exception
{
    public int StatusCode { get; }
    public string Codigo { get; }
    public string Type { get; }
    public string Title { get; }
    public Dictionary<string, string[]>? Errores { get; }

    public ExcepcionApi(
        int statusCode,
        string codigo,
        string message,
        string title = "Error de operación",
        string? type = null,
        Dictionary<string, string[]>? errores = null)
        : base(message)
    {
        StatusCode = statusCode;
        Codigo = codigo;
        Title = title;
        Type = type ?? $"https://mesasitec.local/errores/{codigo.ToLower().Replace('_', '-')}";
        Errores = errores;
    }

    public static ExcepcionApi NoAutenticado(string mensaje = "Token ausente, inválido o expirado.")
        => new(401, "NO_AUTENTICADO", mensaje, "No autenticado");

    public static ExcepcionApi OperacionNoPermitida(string mensaje = "El rol no permite la operación.")
        => new(403, "OPERACION_NO_PERMITIDA", mensaje, "Operación no permitida");

    public static ExcepcionApi RecursoNoEncontrado(string mensaje = "Recurso no encontrado o no pertenece a la organización.")
        => new(404, "RECURSO_NO_ENCONTRADO", mensaje, "Recurso no encontrado");

    public static ExcepcionApi TransicionInvalida(string mensaje = "Transición de estado no permitida.")
        => new(409, "TRANSICION_INVALIDA", mensaje, "Transición inválida");

    public static ExcepcionApi AgenteInvalido(string mensaje = "El agente especificado no es válido.")
        => new(422, "AGENTE_INVALIDO", mensaje, "Agente inválido");

    public static ExcepcionApi MotivoRequerido(string mensaje = "El motivo es requerido o muy corto.")
        => new(422, "MOTIVO_REQUERIDO", mensaje, "Motivo requerido");

    public static ExcepcionApi ParametroInvalido(string mensaje = "Parámetro de consulta fuera de rango.")
        => new(400, "PARAMETRO_INVALIDO", mensaje, "Parámetro inválido");

    public static ExcepcionApi Validacion(string mensaje = "Error de validación de campos.", Dictionary<string, string[]>? errores = null)
        => new(422, "VALIDACION", mensaje, "Error de validación", errores: errores);
}
