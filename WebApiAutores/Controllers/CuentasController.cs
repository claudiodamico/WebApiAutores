using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<CuentasController> _logger;
        public CuentasController(UserManager<IdentityUser> userManager, 
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager, ILogger<CuentasController> logger)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._signInManager = signInManager;
            this._logger = logger;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuarios credencialesUsuarios)
        {
            var usuario = new IdentityUser { UserName = credencialesUsuarios.Email,
                Email = credencialesUsuarios.Email };
            var resultado = await _userManager.CreateAsync(usuario, credencialesUsuarios.Password);

            if (resultado.Succeeded)
            {
                return ConstruirToken(credencialesUsuarios);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuarios credencialesUsuarios)
        {
            var resultado = await _signInManager.PasswordSignInAsync(credencialesUsuarios.Email,
                credencialesUsuarios.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return ConstruirToken(credencialesUsuarios);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        private RespuestaAutenticacion ConstruirToken(CredencialesUsuarios credencialesUsuarios)
        {
            var claims = new List<Claim>()
            {
                new Claim("Email", credencialesUsuarios.Email),
                new Claim("ll", "ll")
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);
            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };

        }
    }
}
