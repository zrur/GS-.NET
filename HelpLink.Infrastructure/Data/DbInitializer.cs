using HelpLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpLink.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HelpLinkDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Usuarios.Any())
                return; // já populado

            // 🔹 País
            var pais = new Pais
            {
                Nome = "Brasil",
                Sigla = "BR",
                CodigoIBGE = "76",
                DataCriacao = DateTime.Now,
                Ativo = 1
            };
            context.Paises.Add(pais);
            context.SaveChanges();

            // 🔹 Estado
            var estado = new Estado
            {
                Nome = "São Paulo",
                Sigla = "SP",
                CodigoIBGE = "35",
                PaisId = pais.Id,
                DataCriacao = DateTime.Now,
                Ativo = 1
            };
            context.Estados.Add(estado);
            context.SaveChanges();

            // 🔹 Cidade
            var cidade = new Cidade
            {
                Nome = "São Paulo",
                CodigoIBGE = "3550308",
                EstadoId = estado.Id,
                DataCriacao = DateTime.Now,
                Ativo = 1
            };
            context.Cidades.Add(cidade);
            context.SaveChanges();

            // 🔹 Bairro
            var bairro = new Bairro
            {
                Nome = "Centro",
                CEPInicial = "01000-000",
                CEPFinal = "01099-999",
                CidadeId = cidade.Id,
                DataCriacao = DateTime.Now,
                Ativo = 1
            };
            context.Bairros.Add(bairro);
            context.SaveChanges();

            // 🔹 Endereço
            var endereco = new Endereco
            {
                Logradouro = "Rua das Flores",
                Numero = "100",
                CEP = "01010-000",
                BairroId = bairro.Id,
                DataCriacao = DateTime.Now,
                Ativo = 1
            };
            context.Enderecos.Add(endereco);
            context.SaveChanges();

            // 🔹 Usuários
            var usuarios = new Usuario[]
            {
                new Usuario
                {
                    Nome = "João Silva",
                    Email = "joao.silva@email.com",
                    Telefone = "11987654321",
                    CPF = "123.456.789-00",
                    DataNascimento = new DateTime(1990, 5, 15),
                    DataCriacao = DateTime.Now,
                    Ativo = 1
                },
                new Usuario
                {
                    Nome = "Maria Santos",
                    Email = "maria.santos@email.com",
                    Telefone = "11876543210",
                    CPF = "987.654.321-00",
                    DataNascimento = new DateTime(1985, 8, 20),
                    DataCriacao = DateTime.Now,
                    Ativo = 1
                }
            };
            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            // 🔹 Instituições (usando EnderecoId válido)
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
                    Verificada = 0,
                    EnderecoId = endereco.Id
                },
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
                    Verificada = 0,
                    EnderecoId = endereco.Id
                }
            };
            context.Instituicoes.AddRange(instituicoes);
            context.SaveChanges();

            // 🔹 Doações
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
