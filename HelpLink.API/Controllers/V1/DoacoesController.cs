using HelpLink.Application.DTOs;
using HelpLink.Domain.Entities;
using HelpLink.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpLink.API.Controllers.V1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class DoacoesController : ControllerBase
{
    private readonly HelpLinkDbContext _context;
    private readonly ILogger<DoacoesController> _logger;

    public DoacoesController(HelpLinkDbContext context, ILogger<DoacoesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<DoacaoDto>), StatusCodes.Status200OK)]
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

    [HttpGet("instituicao/{instituicaoId}")]
    [ProducesResponseType(typeof(PagedResponse<DoacaoDto>), StatusCodes.Status200OK)]
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