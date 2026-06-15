using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aduanas.Aci.Seguridad.Api.Models;

public class UsuarioCredencial
{
    [Key]
    public int IdUsuarioCredencial { get; set; }

    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual Usuario? Usuario { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

    [Required]
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

    public int Iteraciones { get; set; } = 100_000;

    public DateTime FechaUltimoCambio { get; set; }

    public int IntentosFallidos { get; set; } = 0;

    public bool BloqueoTemporal { get; set; } = false;
}
