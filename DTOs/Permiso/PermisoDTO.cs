using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Seguridad.Api.DTOs.Permis
{
    public class PermisoDTO
    {
        public int IdPermiso { get; set; }
        public string CodigoPermiso { get; set; }
        public string Descripcion { get; set; }
        public string? Modulo { get; set; }
        public string? Accion { get; set; }
        public string? Referencia { get; set; }
    }
}
