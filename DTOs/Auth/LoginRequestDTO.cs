using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Seguridad.Api.DTOs.Auth;

public class LoginRequestDTO
{
    [Required(ErrorMessage = "El usuario es requerido")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    //[MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    public string Contrasenia { get; set; } = string.Empty;
}
