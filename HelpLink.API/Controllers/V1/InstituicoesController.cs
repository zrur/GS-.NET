using HelpLink.Application.DTOs;
using HelpLink.Domain.Entities;
using HelpLink.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HelpLink.API.Controllers.V1
{
    /// <summary>
    /// 🏥 Controller para gerenciamento de instituições
    /// </summary>
    /// <remarks>
    /// Este controller permite:
    /// - 📈 Listar instituições com paginação e filtros
    /// - 🔍 Buscar instituição por ID
    /// - ➕ Criar nova instituição
    /// - 🗑️ Desativar instituição (exclusão lógica)
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Tags("🏥 Instituições")]
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
        /// <summary>
        /// 📈 Lista todas as instituições ativas com paginação
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Itens por página (padrão: 10)</param>
        /// <param name="cidade">Filtrar por nome da cidade</param>
        /// <returns>Lista paginada de instituições</returns>
        /// <response code="200">Lista de instituições retornada com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<InstituicaoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
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
                    .Where(i => i.Ativo == 1)
                    .AsQueryable();

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

                        // SEM BOOL!!!
                        Verificada = i.Verificada,

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
        /// <summary>
        /// 🔍 Busca uma instituição pelo ID
        /// </summary>
        /// <param name="id">ID da instituição</param>
        /// <returns>Dados detalhados da instituição</returns>
        /// <response code="200">Instituição encontrada</response>
        /// <response code="404">Instituição não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<InstituicaoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<InstituicaoDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
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
                    Verificada = instituicao.Verificada,

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

                return Ok(ApiResponse<InstituicaoDto>.SuccessResponse(dto, "Instituição encontrada com sucesso"));
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
        /// <summary>
        /// ➕ Cria uma nova instituição
        /// </summary>
        /// <param name="dto">Dados da instituição a ser criada</param>
        /// <returns>Dados da instituição criada</returns>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/v1/instituicoes
        ///     {
        ///       "nome": "Instituto Exemplo",
        ///       "cnpj": "12.345.678/0001-90",
        ///       "email": "contato@instituto-exemplo.org",
        ///       "telefone": "(11) 99999-9999",
        ///       "descricao": "Instituto dedicado a ajudar crianças",
        ///       "enderecoId": null,
        ///       "ativo": 1
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Instituição criada com sucesso</response>
        /// <response code="400">Dados inválidos ou CNPJ já existe</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<InstituicaoDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public async Task<ActionResult<ApiResponse<InstituicaoDto>>> CreateInstituicao(
    [FromBody] InstituicaoCreateDto dto)
{
    try
    {
        // Confere se já existe instituição com o mesmo CNPJ
        if (await _context.Instituicoes.AnyAsync(i => i.CNPJ == dto.CNPJ))
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Já existe uma instituição com este CNPJ"));
        }

        // ✅ AQUI ENTRA O ? (HasValue) — VALIDA O ENDEREÇO
        if (dto.EnderecoId.HasValue)
        {
            bool enderecoExiste = await _context.Enderecos
                .AnyAsync(e => e.Id == dto.EnderecoId.Value);

            if (!enderecoExiste)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Endereço informado não existe"));
            }
        }

        // Agora sim cria a instituição
        var instituicao = new Instituicao
        {
            Nome = dto.Nome,
            CNPJ = dto.CNPJ,
            Descricao = dto.Descricao,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Site = dto.Site,

            // ✅ Usa o EnderecoId do DTO (pode ser null)
            EnderecoId = dto.EnderecoId,

            Ativo = 1,
            Verificada = 0,
            DataCriacao = DateTime.UtcNow
        };

        _context.Instituicoes.Add(instituicao);
        await _context.SaveChangesAsync();

        var retorno = new InstituicaoDto
        {
            Id = instituicao.Id,
            Nome = instituicao.Nome,
            CNPJ = instituicao.CNPJ,
            Verificada = instituicao.Verificada
        };

        return CreatedAtAction(
            nameof(GetInstituicao),
            new { id = instituicao.Id },
            ApiResponse<InstituicaoDto>.SuccessResponse(retorno, "Instituição criada com sucesso")
        );
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao criar instituição");
        return StatusCode(
            500,
            ApiResponse<string>.ErrorResponse(ex.InnerException?.Message ?? ex.Message)
        );
    }
}


        // ============================ DELETE (LOGICO) ============================
        /// <summary>
        /// 🗑️ Desativa uma instituição (exclusão lógica)
        /// </summary>
        /// <param name="id">ID da instituição a ser desativada</param>
        /// <returns>Confirmação da desativação</returns>
        /// <response code="200">Instituição desativada com sucesso</response>
        /// <response code="404">Instituição não encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInstituicao(int id)
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);

            if (instituicao == null)
                return NotFound(ApiResponse<string>.ErrorResponse("Instituição não encontrada"));

            instituicao.Ativo = 0;

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<string>.SuccessResponse(null!, "Instituição desativada com sucesso"));        }

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
