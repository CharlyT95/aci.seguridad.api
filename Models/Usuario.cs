using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aduanas.Aci.Seguridad.Api.Models;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required, MaxLength(100)]
    public string UsuarioLogin { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Nombres { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Apellidos { get; set; }

    [Required, MaxLength(200)]
    public string CorreoElectronico { get; set; } = string.Empty;

    public bool Activo {  get; set; }
}
