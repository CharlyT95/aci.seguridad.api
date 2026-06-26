using Aduanas.Aci.Seguridad.Api.DTOs;

namespace Aduanas.Aci.Seguridad.Api.Helpers
{
    public class ResponseHelper
    {
        public static ApiResponse<T> Success<T>(T data, string message = "Acción exitosa")
        {
            return new ApiResponse<T>
            {
                Resultado = true,
                Datos = data,
                Mensaje = message
            };
        }

        public static ApiResponse<T> Fail<T>(string message)
        {
            return new ApiResponse<T>
            {
                Resultado = false,
                Datos = default,
                Mensaje = message
            };
        }
    }
}
