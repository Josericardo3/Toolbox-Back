using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace inti_repository
{
    public class TokenConfiguration
    {
    

        public string GenerarToken(string Usuario, int Minutos, int id,string issuervalor, string audiencevalor
            , string keyvalor)
        {
            var issuer = issuervalor;
            var audience = audiencevalor;
            var key = Encoding.ASCII.GetBytes
                (keyvalor);


            var claims = new[]
               {
                new Claim("id", id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,Usuario ),
                new Claim(JwtRegisteredClaimNames.Email, Usuario),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat,
                DateTime.UtcNow.ToString())
             };

            var SigningCredentials = new SigningCredentials
               (new SymmetricSecurityKey(key),
               SecurityAlgorithms.HmacSha512Signature);

            var token1 = new JwtSecurityToken(
              issuer,
              audience,
              claims,
              DateTime.UtcNow,
              DateTime.UtcNow.AddMinutes(Minutos),
              SigningCredentials
              );



            return new JwtSecurityTokenHandler().WriteToken(token1);
        }

    }
}
