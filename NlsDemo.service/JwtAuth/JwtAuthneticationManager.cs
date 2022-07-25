using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestAppService.JwtAuth
{
    public class JwtAuthneticationManager : IJwtAuthneticationManager
    {
        #region Properties
        private readonly string key;

        #endregion

        #region Constructor
        public JwtAuthneticationManager(string key)
        {
            this.key = key;
        }
        #endregion

        #region Method

        //Token generation and create auth claim for authorization
        public string Authenticate(string userName, string password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                                     new SymmetricSecurityKey(tokenKey),
                                     SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion


    }
}
