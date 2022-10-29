using API_BD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_BD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        // OBLIGATORIO PARA TODOS LOS CONTROLADORES

        /*
         * Objeto de Contexto
         * Se crea una instancia del contexto de la BD
         */
        public readonly WEB_APIContext _dbcontext;
        // Configuración para los usuarios (appsetings.json)
        public IConfiguration _configuration; 

        /*
         * Cuando se llame el controlador, el constructor asigna
         * automáticamente el parámetro de contexto de la BD
         */
        public UsuariosController(WEB_APIContext _context, IConfiguration _config)
        {
            _dbcontext = _context;
            _configuration = _config;
        }
        // ----------------------------------------

        [HttpPost]
        [Route("Session")]
        public IActionResult Session([FromBody] Usuario objeto)
        {
            try
            {
                Usuario objUser = _dbcontext.Usuarios.Where(u => u.Nombre == objeto.Nombre && u.Contraseña == objeto.Contraseña).FirstOrDefault();

                if (objUser == null)
                {
                    return NotFound("Credenciales Inválidas");
                }

                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                var Claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Nombre", objUser.Nombre),
                    new Claim("Id", objUser.IdUsuario.ToString()),
                    new Claim("Perfil", objUser.Perfil)
                };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                SigningCredentials signCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken jwtToken = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    Claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signCred
                    );

                return StatusCode(StatusCodes.Status200OK, 
                    new { 
                        mensaje = "bien 👍",
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
                    });
            }
            catch
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { mensaje = "XD" });
            }
        }
    }
}
