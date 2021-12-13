using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI_Definitivo
{
    public class MyTokenValidator : JwtSecurityTokenHandler, ISecurityTokenValidator
    {
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            try
            {
                JwtSecurityToken incomingToken = ReadJwtToken(token);
                string userName = incomingToken.Claims.FirstOrDefault(q => q.Type == "Username").Value;
                validationParameters.IssuerSigningKey = SecurityKeyGenerator.GetSecurityKey(userName);
                return base.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception) { validatedToken = null; return new ClaimsPrincipal(); }
        }
    }
}