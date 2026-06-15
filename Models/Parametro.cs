namespace Aduanas.Aci.Seguridad.Api.Models
{
    public class Parametro
    {
        public int IdParametro { get; set; }
        public string CodigoParametro { get; set; } = null!;
        public string Valor { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
