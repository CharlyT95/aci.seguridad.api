using Aduanas.Aci.Seguridad.Api.DTOs.Usuario;

namespace Aduanas.Aci.Seguridad.Api.DTOs.Auth;

public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public string TokenActualizacion { get; set; } = string.Empty;
    public DateTime TokenExpiracion { get; set; }
    public DateTime TokenActualizacionExpiracion { get; set; }
    public UsuarioDTO Usuario { get; set; } = new();
}
