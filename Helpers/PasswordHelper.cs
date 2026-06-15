using System.Security.Cryptography;
using System.Text;

namespace Aduanas.Aci.Seguridad.Api.Helpers;

public static class PasswordHelper
{
    /// <summary>
    /// Verifica la contraseña usando HMACSHA512,
    /// compatible con el PasswordService que crea las credenciales.
    /// </summary>
    public static bool VerificarPassword(
        string password,
        byte[] storedHash,
        byte[] storedSalt,
        int iteraciones) // el parámetro se mantiene por compatibilidad con la firma
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        // Comparación en tiempo constante para evitar timing attacks
        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}