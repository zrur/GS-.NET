using HelpLink.Application.DTOs;
using HelpLink.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpLink.HelpLink.HelpLink.API.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class InstituicoesController : ControllerBase
{
    private readonly HelpLinkDbContext _context;
    private readonly ILogger<InstituicoesController> _logger;

    public InstituicoesController(HelpLinkDbContext context, ILogger<InstituicoesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// [V2] Obtém instituições próximas com base em coordenadas geográficas
    /// </summary>
    [HttpGet("proximas")]
    [ProducesResponseType(typeof(PagedResponse<InstituicaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<InstituicaoDto>>> GetInstituicoesProximas(
        [FromQuery] decimal latitude,
        [FromQuery] decimal longitude,
        [FromQuery] decimal raioKm = 10,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var instituicoes = await _context.Instituicoes
                .Include(i => i.Endereco)
                .Where(i => i.Ativo)
                .ToListAsync();

            var totalRecords = instituicoes.Count;
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var response = new PagedResponse<InstituicaoDto>(
                new List<InstituicaoDto>(),
                totalRecords,
                pageNumber,
                pageSize
            );

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar instituições próximas");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// [V2] Obtém estatísticas de uma instituição
    /// </summary>
    [HttpGet("{id}/estatisticas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetEstatisticas(int id)
    {
        try
        {
            var instituicao = await _context.Instituicoes
                .Include(i => i.Doacoes)
                .FirstOrDefaultAsync(i => i.Id == id && i.Ativo);

            if (instituicao == null)
            {
                return NotFound(new { message = "Instituição não encontrada" });
            }

            var estatisticas = new
            {
                TotalDoacoes = instituicao.Doacoes.Count,
                DoacoesConcluidas = instituicao.Doacoes.Count(d => d.Status == "Concluída")
            };

            return Ok(ApiResponse<object>.SuccessResponse(estatisticas, "Estatísticas obtidas com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Erro interno do servidor" });
        }
    }
}
