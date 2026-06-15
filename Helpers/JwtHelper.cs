using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace Aduanas.Aci.Seguridad.Api.Helpers;
public class JwtHelper
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    public JwtHelper(IConfiguration config)
    {
        _secretKey = config["JwtSettings:SecretKey"]!;
        _issuer = config["JwtSettings:Issuer"]!;
        _audience = config["JwtSettings:Audience"]!;
    }
    public (string token, DateTime expiration) GenerateAccessToken(
        int idUsuario, string usuarioLogin, IEnumerable<string> roles, IEnumerable<string> permisos, int minutos)
    {
        var expiration = DateTime.UtcNow.AddMinutes(minutos);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,        idUsuario.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, usuarioLogin),
            new(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };
        // Roles como claims
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        // Permisos como claims personalizados
        claims.AddRange(permisos.Select(p => new Claim("permiso", p)));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );
        return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }
    public (string token, DateTime expiration) GenerateRefreshToken(int dias)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return (Convert.ToBase64String(randomBytes),
                DateTime.UtcNow.AddDays(dias));
    }
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false, // ignorar expiración
            ValidateIssuerSigningKey = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(_secretKey))
        };
        try
        {
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParams, out var validatedToken);
            if (validatedToken is not JwtSecurityToken jwt ||
                !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                return null;
            return principal;
        }
        catch { return null; }
    }
}