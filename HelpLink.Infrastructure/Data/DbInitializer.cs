using HelpLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpLink.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HelpLinkDbContext context)
        {
            // Garante que o banco foi criado
            context.Database.EnsureCreated();

            // Verifica se já existem dados
            if (context.Usuarios.Any())
            {
                return; // BD já foi populado
            }

            // Seed de Usuários
            var usuarios = new Usuario[]
            {
                new Usuario
                {
                    Nome = "João Silva",
                    Email = "joao.silva@email.com",
                    Telefone = "11987654321",
                    CPF = "123.456.789-00",
                    DataNascimento = new DateTime(1990, 5, 15),
                    DataCriacao = DateTime.Now
                },
                new Usuario
                {
                    Nome = "Maria Santos",
                    Email = "maria.santos@email.com",
                    Telefone = "11876543210",
                    CPF = "987.654.321-00",
                    DataNascimento = new DateTime(1985, 8, 20),
                    DataCriacao = DateTime.Now
                }
            };

            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            // Seed de Instituições
            var instituicoes = new Instituicao[]
            {
                new Instituicao
                {
                    Nome = "ONG Esperança",
                    CNPJ = "12.345.678/0001-90",
                    Descricao = "ONG dedicada a ajudar comunidades carentes",
                    Email = "contato@ongesperanca.org.br",
                    Telefone = "1133334444",
                    Site = "www.ongesperanca.org.br",
                    DataCriacao = DateTime.Now,
                    Ativo = 1,
                    Verificada = 0                },
                new Instituicao
                {
                    Nome = "Instituto Solidário",
                    CNPJ = "98.765.432/0001-10",
                    Descricao = "Instituto focado em educação e cultura",
                    Email = "contato@institutosolidario.org.br",
                    Telefone = "1155556666",
                    Site = "www.institutosolidario.org.br",
                    DataCriacao = DateTime.Now,
                    Ativo = 1,
                    Verificada = 0                }
            };

            context.Instituicoes.AddRange(instituicoes);
            context.SaveChanges();

            // Seed de Doações
            var doacoes = new Doacao[]
            {
                new Doacao
                {
                    UsuarioId = usuarios[0].Id,
                    InstituicaoId = instituicoes[0].Id,
                    DataDoacao = DateTime.Now.AddDays(-10),
                    Status = "Concluída",
                    Observacoes = "Primeira doação de teste",
                    DataCriacao = DateTime.Now.AddDays(-10)
                },
                new Doacao
                {
                    UsuarioId = usuarios[0].Id,
                    InstituicaoId = instituicoes[1].Id,
                    DataDoacao = DateTime.Now.AddDays(-5),
                    Status = "Concluída",
                    Observacoes = "Segunda doação de teste",
                    DataCriacao = DateTime.Now.AddDays(-5)
                }
            };

            context.Doacoes.AddRange(doacoes);
            context.SaveChanges();
        }
    }
}
