using HelpLink.Application.DTOs;
using HelpLink.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLink.API.Controllers
{
    /// <summary>
    /// 🔐 Controller de autenticação e autorização
    /// </summary>
    /// <remarks>
    /// Este controller permite:
    /// - 🔑 Login com email e senha
    /// - 🎯 Geração de tokens JWT
    /// - 🔒 Validação de autenticação
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("🔐 Autenticação")]
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
        /// 🔑 Realiza login e retorna token JWT
        /// </summary>
        /// <param name="loginDto">Credenciais de login</param>
        /// <returns>Token JWT para autenticação</returns>
        /// <remarks>
        /// Credenciais de teste:
        /// 
        ///     POST /api/Auth/login
        ///     {
        ///       "email": "admin@helplink.com",
        ///       "password": "Admin@123"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Login realizado com sucesso, token retornado</response>
        /// <response code="401">Email ou senha inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// 🔒 Endpoint protegido para teste de autenticação
        /// </summary>
        /// <returns>Informações do usuário autenticado</returns>
        /// <remarks>
        /// Este endpoint requer autenticação JWT.
        /// Use o token obtido no login no cabeçalho Authorization.
        /// </remarks>
        /// <response code="200">Usuário autenticado com sucesso</response>
        /// <response code="401">Token inválido ou ausente</response>
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
