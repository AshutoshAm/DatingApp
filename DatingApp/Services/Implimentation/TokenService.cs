using DatingApp.Entities;
using DatingApp.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Services.Implimentation
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration _config)
        {
            var tknstr = _config["TokenKey"];
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tknstr));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub,"Kuch bhi"),

            };


            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokendescriptor);

            string tokenstr = tokenhandler.WriteToken(token);

            return tokenstr;
        }
    }
}
