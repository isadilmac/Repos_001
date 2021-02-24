using I_Dilmac_Asp_net_core_5._0_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace I_Dilmac_Asp_net_core_5._0_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Repos005Context _context;

        public AuthenticationController(IConfiguration configuration, Repos005Context context)
        {
            _config = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ObjectResult> Login([FromBody] User _user)
        {
            try
            {
                if (_user.Email != null && _user.Password != null)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(c => c.UserName == _user.Email && c.Password == _user.Password);
                    if (user != null)
                    {
                        return StatusCode(200, new { message = "Giriş Başarılı", token = GenerateJWT(user.UserName,user.Email) });
                    }
                    else
                    {
                        return StatusCode(400, new { message = "Kullanıcı adı veya şifre yanlış!" });
                    }
                }
                else
                {
                    return StatusCode(400, new { message = "Kullanıcı adı ve şifre boş geçilemez." });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        #region helper_method
        private string GenerateJWT(string username, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[] {
                new Claim("username", username),
                new Claim("email", email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
