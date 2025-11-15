using HelpLink.Application.DTOs;
using HelpLink.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLink.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Realiza login e retorna token JWT
        /// </summary>
        /// <param name="loginDto">Credenciais de login</param>
        /// <returns>Token JWT</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = _authService.Login(loginDto);

                if (result == null)
                {
                    _logger.LogWarning("Tentativa de login falhou para: {Email}", loginDto.Email);
                    return Unauthorized(new { message = "Email ou senha inválidos" });
                }

                _logger.LogInformation("Login bem-sucedido para: {Email}", loginDto.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar login");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Endpoint protegido para teste de autenticação
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetCurrentUser()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            return Ok(new { email, message = "Você está autenticado!" });
        }
    }
}
