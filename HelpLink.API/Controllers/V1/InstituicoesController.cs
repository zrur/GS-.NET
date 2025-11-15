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
public class InstituicoesController : ControllerBase
{
    private readonly HelpLinkDbContext _context;
    private readonly ILogger<InstituicoesController> _logger;

    public InstituicoesController(HelpLinkDbContext context, ILogger<InstituicoesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<InstituicaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<InstituicaoDto>>> GetInstituicoes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? cidade = null)
    {
        try
        {
            var query = _context.Instituicoes
                .Include(i => i.Endereco)
                    .ThenInclude(e => e.Bairro)
                        .ThenInclude(b => b.Cidade)
                            .ThenInclude(c => c.Estado)
                .Where(i => i.Ativo);

            if (!string.IsNullOrEmpty(cidade))
            {
                query = query.Where(i => i.Endereco.Bairro.Cidade.Nome.Contains(cidade));
            }

            var totalRecords = await query.CountAsync();

            var instituicoes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new InstituicaoDto
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    CNPJ = i.CNPJ,
                    Descricao = i.Descricao,
                    Email = i.Email,
                    Telefone = i.Telefone,
                    Site = i.Site,
                    Logo = i.Logo,
                    Verificada = i.Verificada,
                    Endereco = new EnderecoDto
                    {
                        Id = i.Endereco.Id,
                        Logradouro = i.Endereco.Logradouro,
                        Numero = i.Endereco.Numero,
                        Complemento = i.Endereco.Complemento,
                        CEP = i.Endereco.CEP,
                        Bairro = i.Endereco.Bairro.Nome,
                        Cidade = i.Endereco.Bairro.Cidade.Nome,
                        Estado = i.Endereco.Bairro.Cidade.Estado.Sigla,
                        Latitude = i.Endereco.Latitude,
                        Longitude = i.Endereco.Longitude
                    }
                })
                .ToListAsync();

            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var links = GeneratePaginationLinks(pageNumber, pageSize, totalPages, cidade).ToList();

            var response = new PagedResponse<InstituicaoDto>(instituicoes, pageNumber, pageSize, totalRecords, links);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter instituições");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    Success = false,
                    Message = "Erro interno do servidor",
                    Data = null!
                });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<InstituicaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<InstituicaoDto>>> GetInstituicao(int id)
    {
        try
        {
            var instituicao = await _context.Instituicoes
                .Include(i => i.Endereco)
                    .ThenInclude(e => e.Bairro)
                        .ThenInclude(b => b.Cidade)
                            .ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(i => i.Id == id && i.Ativo);

            if (instituicao == null)
            {
                return NotFound(new ApiResponse<InstituicaoDto>
                {
                    Success = false,
                    Message = "Instituição não encontrada",
                    Data = null!
                });
            }

            var dto = new InstituicaoDto
            {
                Id = instituicao.Id,
                Nome = instituicao.Nome,
                CNPJ = instituicao.CNPJ,
                Descricao = instituicao.Descricao,
                Email = instituicao.Email,
                Telefone = instituicao.Telefone,
                Site = instituicao.Site,
                Logo = instituicao.Logo,
                Verificada = instituicao.Verificada,
                Endereco = new EnderecoDto
                {
                    Id = instituicao.Endereco.Id,
                    Logradouro = instituicao.Endereco.Logradouro,
                    Numero = instituicao.Endereco.Numero,
                    Complemento = instituicao.Endereco.Complemento,
                    CEP = instituicao.Endereco.CEP,
                    Bairro = instituicao.Endereco.Bairro.Nome,
                    Cidade = instituicao.Endereco.Bairro.Cidade.Nome,
                    Estado = instituicao.Endereco.Bairro.Cidade.Estado.Sigla,
                    Latitude = instituicao.Endereco.Latitude,
                    Longitude = instituicao.Endereco.Longitude
                }
            };

            return Ok(new ApiResponse<InstituicaoDto>
            {
                Success = true,
                Message = "Instituição encontrada com sucesso",
                Data = dto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter instituição {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    Success = false,
                    Message = "Erro interno do servidor",
                    Data = null!
                });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<InstituicaoDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<InstituicaoDto>>> CreateInstituicao(
        [FromBody] InstituicaoCreateDto dto)
    {
        try
        {
            var endereco = await _context.Enderecos.FindAsync(dto.EnderecoId);
            if (endereco == null)
            {
                return BadRequest(new ApiResponse<InstituicaoDto>
                {
                    Success = false,
                    Message = "Endereço não encontrado",
                    Data = null!,
                    Errors = new List<string> { "O endereço informado não existe" }
                });
            }

            if (await _context.Instituicoes.AnyAsync(i => i.CNPJ == dto.CNPJ))
            {
                return BadRequest(new ApiResponse<InstituicaoDto>
                {
                    Success = false,
                    Message = "CNPJ já cadastrado",
                    Data = null!,
                    Errors = new List<string> { "Já existe uma instituição com este CNPJ" }
                });
            }

            var instituicao = new Instituicao
            {
                Nome = dto.Nome,
                CNPJ = dto.CNPJ,
                Descricao = dto.Descricao,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Site = dto.Site,
                EnderecoId = dto.EnderecoId,
                DataCriacao = DateTime.UtcNow
            };

            _context.Instituicoes.Add(instituicao);
            await _context.SaveChangesAsync();

            await _context.Entry(instituicao)
                .Reference(i => i.Endereco)
                .Query()
                .Include(e => e.Bairro)
                    .ThenInclude(b => b.Cidade)
                        .ThenInclude(c => c.Estado)
                .LoadAsync();

            var resultDto = new InstituicaoDto
            {
                Id = instituicao.Id,
                Nome = instituicao.Nome,
                CNPJ = instituicao.CNPJ,
                Descricao = instituicao.Descricao,
                Email = instituicao.Email,
                Telefone = instituicao.Telefone,
                Site = instituicao.Site,
                Logo = instituicao.Logo,
                Verificada = instituicao.Verificada,
                Endereco = new EnderecoDto
                {
                    Id = instituicao.Endereco.Id,
                    Logradouro = instituicao.Endereco.Logradouro,
                    Numero = instituicao.Endereco.Numero,
                    Complemento = instituicao.Endereco.Complemento,
                    CEP = instituicao.Endereco.CEP,
                    Bairro = instituicao.Endereco.Bairro.Nome,
                    Cidade = instituicao.Endereco.Bairro.Cidade.Nome,
                    Estado = instituicao.Endereco.Bairro.Cidade.Estado.Sigla,
                    Latitude = instituicao.Endereco.Latitude,
                    Longitude = instituicao.Endereco.Longitude
                }
            };

            return CreatedAtAction(nameof(GetInstituicao), new { id = instituicao.Id }, new ApiResponse<InstituicaoDto>
            {
                Success = true,
                Message = "Instituição criada com sucesso",
                Data = resultDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar instituição");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    Success = false,
                    Message = "Erro interno do servidor",
                    Data = null!
                });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateInstituicao(int id, [FromBody] InstituicaoUpdateDto dto)
    {
        try
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null || !instituicao.Ativo)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Instituição não encontrada",
                    Data = null!
                });
            }

            if (!string.IsNullOrEmpty(dto.Nome))
                instituicao.Nome = dto.Nome;

            if (!string.IsNullOrEmpty(dto.Descricao))
                instituicao.Descricao = dto.Descricao;

            if (!string.IsNullOrEmpty(dto.Email))
                instituicao.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Telefone))
                instituicao.Telefone = dto.Telefone;

            if (dto.Site != null)
                instituicao.Site = dto.Site;

            if (dto.Logo != null)
                instituicao.Logo = dto.Logo;

            instituicao.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar instituição {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    Success = false,
                    Message = "Erro interno do servidor",
                    Data = null!
                });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInstituicao(int id)
    {
        try
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null || !instituicao.Ativo)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Instituição não encontrada",
                    Data = null!
                });
            }

            instituicao.Ativo = false;
            instituicao.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover instituição {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    Success = false,
                    Message = "Erro interno do servidor",
                    Data = null!
                });
        }
    }

    private IEnumerable<Link> GeneratePaginationLinks(int pageNumber, int pageSize, int totalPages, string? cidade)
    {
        var links = new List<Link>
        {
            new Link
            {
                Href = Url.Action(nameof(GetInstituicoes), new { pageNumber, pageSize, cidade }) ?? "",
                Rel = "self",
                Method = "GET"
            }
        };

        if (pageNumber > 1)
        {
            links.Add(new Link
            {
                Href = Url.Action(nameof(GetInstituicoes), new { pageNumber = pageNumber - 1, pageSize, cidade }) ?? "",
                Rel = "previous",
                Method = "GET"
            });
        }

        if (pageNumber < totalPages)
        {
            links.Add(new Link
            {
                Href = Url.Action(nameof(GetInstituicoes), new { pageNumber = pageNumber + 1, pageSize, cidade }) ?? "",
                Rel = "next",
                Method = "GET"
            });
        }

        return links;
    }
}
