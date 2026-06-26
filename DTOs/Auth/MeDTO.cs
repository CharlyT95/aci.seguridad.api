namespace Aduanas.Aci.Seguridad.Api.DTOs.Auth
{
    public class MeDTO
    {
        public string idUsuario { get; set; }
        public string usuarioLogin { get; set; }
        public List<string> roles { get; set; }
        public List<string> permisos { get; set; }
    }
}
