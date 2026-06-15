using Aduanas.Aci.Seguridad.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Seguridad.Api.Services
{

    public interface IParametroService
    {
        Task<string?> ObtenerValorAsync(string codigoParametro);
    }

    public class ParametroService : IParametroService
    {

        private readonly AppDbContext _context;

        public ParametroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ObtenerValorAsync(string codigoParametro)
        {
            var parametro = await _context.Parametro
                .Where(p => p.CodigoParametro == codigoParametro && p.Activo)
                .Select(p => p.Valor)
                .FirstOrDefaultAsync();

            return parametro;
        }



    }
}
