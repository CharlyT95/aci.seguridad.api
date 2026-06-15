using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aduanas.Aci.Seguridad.Api.Models;

public class Permiso
{
    [Key]
    public int IdPermiso { get; set; }

    [Required, MaxLength(100)]
    public string CodigoPermiso { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Descripcion { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Modulo { get; set; }

    [MaxLength(100)]
    public string? Accion { get; set; }

    [MaxLength(300)]
    public string? Referencia { get; set; }

    public bool Activo { get; set; }
}
