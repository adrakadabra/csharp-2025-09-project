using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using OrderPickingService.Api.Models;

namespace OrderPickingService.Api.Services;

public class KeycloakRolesTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = principal.Identity as ClaimsIdentity;
        
        if (claimsIdentity == null)
            return Task.FromResult(principal);
        
        var realmAccessClaim = principal.FindFirst("realm_access");
        if (realmAccessClaim != null)
        {
            try
            {
                var realmAccess = JsonSerializer.Deserialize<RealmAccess>(realmAccessClaim.Value); 
                
                if (realmAccess?.Roles != null)
                {
                    foreach (var role in realmAccess.Roles)
                    {
                        if (!claimsIdentity.HasClaim(ClaimTypes.Role, role))
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing realm_access: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("realm_access claim not found");
        }

        return Task.FromResult(principal);
    }
}