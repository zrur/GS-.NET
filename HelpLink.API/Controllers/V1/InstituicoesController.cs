using HelpLink.Application.DTOs;
using HelpLink.Domain.Entities;
using HelpLink.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpLink.API.Controllers.V1
{
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

        // ============================ GET ALL ============================
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
                    .Where(i => i.Ativo == 1);

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
                        Verificada = i.Verificada == 1,

                        Endereco = i.Endereco == null ? null : new EnderecoDto
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

                return Ok(new PagedResponse<InstituicaoDto>(instituicoes, pageNumber, pageSize, totalRecords, links));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter instituições");
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Erro interno do servidor.",
                    Data = null!
                });
            }
        }

        // ============================ GET BY ID ============================
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<InstituicaoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<InstituicaoDto>>> GetInstituicao(int id)
        {
            try
            {
                var instituicao = await _context.Instituicoes
                    .Include(i => i.Endereco)
                        .ThenInclude(e => e.Bairro)
                            .ThenInclude(b => b.Cidade)
                                .ThenInclude(c => c.Estado)
                    .FirstOrDefaultAsync(i => i.Id == id && i.Ativo == 1);

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
                    Verificada = instituicao.Verificada ==1,
                    Endereco = instituicao.Endereco == null ? null : new EnderecoDto
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
                _logger.LogError(ex, "Erro ao buscar instituição");
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Erro interno do servidor",
                    Data = null!
                });
            }
        }

        // ============================ POST ============================
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<InstituicaoDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<InstituicaoDto>>> CreateInstituicao(
            [FromBody] InstituicaoCreateDto dto)
        {
            try
            {
                Endereco? endereco = null;

                if (dto.EnderecoId.HasValue)
                {
                    endereco = await _context.Enderecos.FindAsync(dto.EnderecoId.Value);

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
                    DataCriacao = DateTime.UtcNow,
                    Ativo =1 
                };

                _context.Instituicoes.Add(instituicao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInstituicao), new { id = instituicao.Id }, new ApiResponse<InstituicaoDto>
                {
                    Success = true,
                    Message = "Instituição criada com sucesso",
                    Data = new InstituicaoDto
                    {
                        Id = instituicao.Id,
                        Nome = instituicao.Nome
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar instituição");

                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null!
                });
            }
        }

        // ============================ PAGINATION ============================
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
}
