using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

public class SnoClaimsTransformation : IClaimsTransformation
{
    
    private ClaimsIdentity identity;
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal user)
    {
        identity = new ClaimsIdentity([new Claim(ClaimsIdentity.DefaultRoleClaimType, "")])
        
        if(!user.HasClaim(claim => claim.Type == "UserID"))
        {
            
             
        }
    }
}