using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aduanas.Aci.Seguridad.Api.Models;

public class Rol
{
    [Key]
    public int IdRol { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Descripcion { get; set; } = string.Empty;

    public bool Activo { get; set; }
}
