using APICatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em : " + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody]UsuarioDTO model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user,false);
            return Ok(GeraToken(model));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e=> e.Errors));
            }

            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(GeraToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty,"Login Inválido...");
                return BadRequest(ModelState);
            }
        }

        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("meuPet","Loro"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //tempo de expiração
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims:claims,
                expires: expiration,
                signingCredentials: credenciais);

            return new UsuarioToken() 
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Experiention = expiration,
                Message = "Token JWT Ok"
            };
        }
    }
}
