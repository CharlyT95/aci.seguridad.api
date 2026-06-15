using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aduanas.Aci.Seguridad.Api.Models;

public class UsuarioRol
{
    [Key]
    public int IdUsuarioRol { get; set; }

    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual Usuario? Usuario { get; set; }

    public int IdRol { get; set; }

    [ForeignKey("IdRol")]
    public virtual Rol? Rol { get; set; }

    public bool Activo {  get; set; }
}
