using HelpLink.Application.DTOs;
using HelpLink.Domain.Entities;
using HelpLink.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpLink.API.Controllers.V1;


/// <summary>
/// 🤝 Controller para gerenciamento de doações
/// </summary>
/// <remarks>
/// Este controller permite:
/// - 📈 Listar doações com paginação e filtros
/// - 🔍 Buscar doações por instituição
/// - 📊 Visualizar relatórios de doações
/// </remarks>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Tags("🤝 Doações")]
public class DoacoesController : ControllerBase
{
    private readonly HelpLinkDbContext _context;
    private readonly ILogger<DoacoesController> _logger;

    public DoacoesController(HelpLinkDbContext context, ILogger<DoacoesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 📈 Lista todas as doações ativas com paginação
    /// </summary>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Itens por página (padrão: 10)</param>
    /// <param name="status">Filtrar por status da doação</param>
    /// <returns>Lista paginada de doações</returns>
    /// <response code="200">Lista de doações retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<DoacaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<DoacaoDto>>> GetDoacoes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null)
    {
        try
        {
            var query = _context.Doacoes
                .Include(d => d.Usuario)
                .Include(d => d.Instituicao)
                .Include(d => d.DoacaoItens)
                .ThenInclude(di => di.Item)
                .ThenInclude(i => i.Categoria)
                .Include(d => d.Agendamento)
                .Where(d => d.Ativo == 1);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(d => d.Status == status);
            }

            var totalRecords = await query.CountAsync();
            var doacoes = await query
                .OrderByDescending(d => d.DataDoacao)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DoacaoDto
                {
                    Id = d.Id,
                    DataDoacao = d.DataDoacao,
                    Status = d.Status,
                    Observacoes = d.Observacoes,
                    ValorEstimado = d.ValorEstimado,
                    UsuarioId = d.UsuarioId,
                    NomeUsuario = d.Usuario.Nome,
                    InstituicaoId = d.InstituicaoId,
                    NomeInstituicao = d.Instituicao.Nome,
                    Itens = d.DoacaoItens.Select(di => new DoacaoItemDto
                    {
                        Id = di.Id,
                        ItemId = di.ItemId,
                        NomeItem = di.Item.Nome,
                        CategoriaItem = di.Item.Categoria.Nome,
                        Quantidade = di.Quantidade,
                        Observacoes = di.Observacoes
                    }),
                    Agendamento = d.Agendamento != null ? new AgendamentoDto
                    {
                        Id = d.Agendamento.Id,
                        DataAgendada = d.Agendamento.DataAgendada,
                        HorarioInicio = d.Agendamento.HorarioInicio,
                        HorarioFim = d.Agendamento.HorarioFim,
                        Status = d.Agendamento.Status,
                        Observacoes = d.Agendamento.Observacoes
                    } : null
                })
                .ToListAsync();

            var response = new PagedResponse<DoacaoDto>(doacoes, pageNumber, pageSize, totalRecords);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter doações");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// 🏥 Lista doações de uma instituição específica
    /// </summary>
    /// <param name="instituicaoId">ID da instituição</param>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Itens por página (padrão: 10)</param>
    /// <returns>Lista paginada de doações da instituição</returns>
    /// <response code="200">Lista de doações da instituição retornada com sucesso</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("instituicao/{instituicaoId}")]
    [ProducesResponseType(typeof(PagedResponse<DoacaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<DoacaoDto>>> GetDoacoesByInstituicao(
        int instituicaoId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = _context.Doacoes
                .Include(d => d.Usuario)
                .Include(d => d.Instituicao)
                .Include(d => d.DoacaoItens)
                .ThenInclude(di => di.Item)
                .ThenInclude(i => i.Categoria)
                .Where(d => d.InstituicaoId == instituicaoId && d.Ativo == 1);

            var totalRecords = await query.CountAsync();
            var doacoes = await query
                .OrderByDescending(d => d.DataDoacao)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DoacaoDto
                {
                    Id = d.Id,
                    DataDoacao = d.DataDoacao,
                    Status = d.Status,
                    Observacoes = d.Observacoes,
                    ValorEstimado = d.ValorEstimado,
                    UsuarioId = d.UsuarioId,
                    NomeUsuario = d.Usuario.Nome,
                    InstituicaoId = d.InstituicaoId,
                    NomeInstituicao = d.Instituicao.Nome,
                    Itens = d.DoacaoItens.Select(di => new DoacaoItemDto
                    {
                        Id = di.Id,
                        ItemId = di.ItemId,
                        NomeItem = di.Item.Nome,
                        CategoriaItem = di.Item.Categoria.Nome,
                        Quantidade = di.Quantidade,
                        Observacoes = di.Observacoes
                    })
                })
                .ToListAsync();

            var response = new PagedResponse<DoacaoDto>(doacoes, pageNumber, pageSize, totalRecords);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter doações da instituição {InstituicaoId}", instituicaoId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Erro interno do servidor" });
        }
    }
}