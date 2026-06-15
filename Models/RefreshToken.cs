using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aduanas.Aci.Seguridad.Api.Models;

[Table("RefreshToken")]
public class RefreshToken
{
    [Key]
    [Column("TokenId")]
    public int IdRefreshToken { get; set; }

    [Column("IdUsuario")]
    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual Usuario? Usuario { get; set; }

    [Column("TokenHash")]
    public string TokenHash { get; set; } = string.Empty;

    [Column("TokenHashReemplazo")]
    public string? TokenHashReemplazo { get; set; }

    [Column("FechaCreado")]
    public DateTime FechaCreado { get; set; } = DateTime.UtcNow;

    [Column("Expira")]
    public DateTime Expira { get; set; }

    [Column("Revocado")]
    public bool Revocado { get; set; } = false;

    [Column("FechaRevocado")]
    public DateTime? FechaRevocado { get; set; }
}