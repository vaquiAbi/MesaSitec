namespace Api.Modelos.Dto.Response;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiraEn { get; set; } 
    public UsuarioDto Usuario { get; set; } = default!;
}
