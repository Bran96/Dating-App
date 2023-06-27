using DatingAppApi.Entities;
using DatingAppApi.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingAppApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config) {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        // Adding Logic to the 'CreateToken' method to be able to create a JSON WEB TOKEN that we can return to our client
        // We must get a package called "System.IdentityModel.Tokens.Jwt" from Nuget Package Manager and make use of it in this service file
        public string CreateToken(AppUser user)
        {
            // These are all claims who the user is / Information about the user
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()), // This is to get the user by id
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName) // This is toget the user by username

            };

            // Signing this Token with
            // 2nd Parameter is what algorithm I use to eencrypt this particular key
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Now We gonna describe the token we gonna return
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            // Now we need a token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
