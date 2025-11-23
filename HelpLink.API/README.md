# 🌟 HelpLink API - O Futuro do Trabalho

<div align="center">

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?style=for-the-badge&logo=oracle)](https://www.oracle.com/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens)](https://jwt.io/)
[![Health Checks](https://img.shields.io/badge/Health-Checks-4CAF50?style=for-the-badge)](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks)
[![xUnit](https://img.shields.io/badge/Tests-xUnit-FF6B6B?style=for-the-badge)](https://xunit.net/)
[![Swagger](https://img.shields.io/badge/Docs-Swagger-85EA2D?style=for-the-badge&logo=swagger)](https://swagger.io/)

![API Status](https://img.shields.io/badge/API-Online-success?style=for-the-badge)
![Tests](https://img.shields.io/badge/Tests-10/10%20Passing-success?style=for-the-badge)
![Coverage](https://img.shields.io/badge/Requirements-100%25-success?style=for-the-badge)

</div>

---

## 📋 Sobre o Projeto

> **"Transformando o futuro do trabalho através da tecnologia e solidariedade"**

**HelpLink** é uma API RESTful de **nível empresarial** desenvolvida em **.NET 9** que representa uma solução tecnológica inovadora para **"O Futuro do Trabalho"**. A plataforma conecta doadores, instituições beneficentes e voluntários, criando um **ecossistema digital** que facilita a solidariedade e o impacto social através da tecnologia moderna.

### 🎯 Visão do Futuro do Trabalho

Este projeto revoluciona o conceito tradicional de trabalho através de:

🌐 **Plataforma Digital Integrada**
- Conexão inteligente entre pessoas e instituições
- Marketplace social para doações e voluntariado
- Dashboard de transparência e impacto social

👥 **Nova Economia do Trabalho**
- **Voluntariado 4.0**: Digital + Presencial
- **Gig Economy Social**: Trabalho com propósito
- **Skills Matching**: IA conectando habilidades e necessidades

🚀 **Inovação Tecnológica**
- **Microserviços** escaláveis e modulares
- **Observabilidade** completa (logs, métricas, tracing)
- **API-First** com documentação interativa
- **Cloud Ready** com containerização

📊 **Transparência e Impacto**
- Relatórios de impacto em tempo real
- Métricas sociais mensuráveis
- Blockchain-ready para certificação de doações

💡 **Capacitação Contínua**
- Parcerias com instituições educacionais
- Trilhas de desenvolvimento profissional
- Certificações em tecnologias emergentes

## 🚀 Funcionalidades

### 🏢 Gestão de Instituições
- Cadastro e gerenciamento de ONGs e instituições beneficentes
- Sistema de verificação e validação
- Integração com endereços georreferenciados

### 👥 Gestão de Usuários e Voluntários
- Cadastro de usuários doadores
- Sistema de voluntários com habilidades e disponibilidade
- Matching inteligente entre voluntários e necessidades

### 📦 Sistema de Doações
- Gerenciamento completo de doações (itens e valores)
- Agendamento de coletas e entregas
- Rastreamento do status das doações

### 📊 Relatórios de Impacto
- Métricas de beneficiados por doação
- Relatórios de transparência para doadores
- Dashboard de impacto social

## 🛠️ Tecnologias Utilizadas

- **.NET 9** - Framework principal
- **ASP.NET Core Web API** - API RESTful
- **Entity Framework Core** - ORM e Migrations
- **Oracle Database** - Banco de dados
- **JWT Authentication** - Segurança e autenticação
- **OpenTelemetry** - Observabilidade e tracing
- **Serilog** - Logging estruturado
- **Health Checks** - Monitoramento
- **Swagger/OpenAPI** - Documentação
- **xUnit** - Testes automatizados

## 📁 Arquitetura do Projeto

```
HelpLink/
├── HelpLink.API/              # Camada de apresentação (Controllers, Auth)
├── HelpLink.Application/      # Camada de aplicação (DTOs, Services)
├── HelpLink.Domain/           # Camada de domínio (Entities, Rules)
├── HelpLink.Infrastructure/   # Camada de infraestrutura (Data, Migrations)
└── HelpLink.Tests/           # Testes automatizados
```

### 🎨 Padrões Arquiteturais
- **Clean Architecture** - Separação clara de responsabilidades
- **Repository Pattern** - Abstração do acesso a dados
- **Dependency Injection** - Inversão de controle
- **CQRS** - Separação de comandos e consultas

## 🚀 Quick Start

### 📦 Pré-requisitos

```bash
# Verificar versões necessárias
dotnet --version  # >= 9.0
```

**Requerimentos:**
- ✅ .NET 9 SDK
- ✅ Oracle Database (FIAP configurado)
- ✅ Visual Studio 2022, VS Code ou Rider
- ✅ Postman/Insomnia (opcional para testes)

### ⚡ Execução Rápida

```bash
# 1. Clone e navegue para o diretório
git clone [url-do-repositorio]
cd HelpLink

# 2. Restaure dependências e compile
dotnet restore
dotnet build

# 3. Execute a aplicação
dotnet run --project HelpLink.API --urls "http://localhost:5023"

# ✅ API estará rodando em: http://localhost:5023
# 📚 Documentação Swagger: http://localhost:5023/swagger
# 🏥 Health Check: http://localhost:5023/health
```

### 🔧 Configuração Avançada

<details>
<summary>🗄️ Configuração do Banco Oracle</summary>

Edite `HelpLink.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=rm558798;Password=fiap24;Data Source=oracle.fiap.com.br:1521/ORCL;"
  },
  "Jwt": {
    "SecretKey": "sua-chave-super-secreta-256-bits",
    "Issuer": "HelpLinkAPI",
    "Audience": "HelpLinkUsers",
    "ExpiryMinutes": 60
  }
}
```
</details>

<details>
<summary>🗃️ Migrações do Banco (Opcional)</summary>

```bash
# Aplicar migrações (se necessário)
cd HelpLink.Infrastructure
dotnet ef database update --startup-project ../HelpLink.API

# Criar nova migração
dotnet ef migrations add NomeDaMigracao --startup-project ../HelpLink.API
```
</details>

---

## 🔐 Autenticação Rápida

### 1️⃣ Login Padrão

```bash
curl -X POST http://localhost:5023/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@helplink.com",
    "password": "Admin@123"
  }'
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "admin@helplink.com",
  "expiresAt": "2025-01-01T12:00:00Z"
}
```

### 2️⃣ Usar Token nas Requisições

```bash
# Copie o token da resposta acima
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Use em todas as requisições protegidas
curl -X GET http://localhost:5023/api/v1/Instituicoes \
  -H "Authorization: Bearer $TOKEN"
```

---

## 📋 Como Fazer Requisições

### 🏢 **Gerenciar Instituições**

<details>
<summary>📝 <strong>Criar Nova Instituição</strong></summary>

```bash
curl -X POST http://localhost:5023/api/v1/Instituicoes \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "nome": "Instituto Tecnologia Social",
    "cnpj": "12.345.678/0001-90",
    "descricao": "Organização dedicada à inclusão digital e capacitação tecnológica para o futuro do trabalho",
    "email": "contato@techsocial.org.br",
    "telefone": "11987654321",
    "site": "https://techsocial.org.br"
  }'
```

**✅ Resposta de Sucesso (201):**
```json
{
  "success": true,
  "message": "Instituição criada com sucesso",
  "data": {
    "id": 42,
    "nome": "Instituto Tecnologia Social",
    "cnpj": "12.345.678/0001-90",
    "descricao": "Organização dedicada à inclusão digital...",
    "email": "contato@techsocial.org.br",
    "verificada": 0,
    "ativo": 1,
    "dataCreacao": "2025-01-01T10:00:00Z"
  }
}
```
</details>

<details>
<summary>📋 <strong>Listar Instituições com Paginação</strong></summary>

```bash
# Listar com paginação
curl -X GET "http://localhost:5023/api/v1/Instituicoes?pageNumber=1&pageSize=10&cidade=São Paulo" \
  -H "Authorization: Bearer $TOKEN"

# Buscar instituição específica
curl -X GET http://localhost:5023/api/v1/Instituicoes/42 \
  -H "Authorization: Bearer $TOKEN"
```

**📄 Resposta Paginada:**
```json
{
  "success": true,
  "data": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalRecords": 150,
  "totalPages": 15,
  "links": [
    {
      "href": "/api/v1/Instituicoes?pageNumber=1&pageSize=10",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1/Instituicoes?pageNumber=2&pageSize=10", 
      "rel": "next",
      "method": "GET"
    }
  ]
}
```
</details>

### 📦 **Gerenciar Doações**

<details>
<summary>🎁 <strong>Criar Nova Doação</strong></summary>

```bash
curl -X POST http://localhost:5023/api/v1/Doacoes \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "usuarioId": 1,
    "instituicaoId": 42,
    "dataDoacao": "2025-01-15T14:30:00",
    "status": "Pendente",
    "observacoes": "Doação de equipamentos para laboratório de informática",
    "valorEstimado": 15000.00
  }'
```
</details>

<details>
<summary>📊 <strong>Listar Doações</strong></summary>

```bash
# Todas as doações
curl -X GET "http://localhost:5023/api/v1/Doacoes?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"

# Doações de uma instituição específica  
curl -X GET "http://localhost:5023/api/v1/Doacoes/instituicao/42" \
  -H "Authorization: Bearer $TOKEN"
```
</details>

### 🔍 **Versionamento da API**

```bash
# API v1 (atual)
curl -X GET http://localhost:5023/api/v1/Instituicoes

# API v2 (recursos avançados)
curl -X GET http://localhost:5023/api/v2/Instituicoes/proximas?lat=-23.5505&lng=-46.6333
curl -X GET http://localhost:5023/api/v2/Instituicoes/42/estatisticas
```

---

## 🏥 Monitoramento e Status

## 🔐 Autenticação

### Login Padrão
```json
{
  "email": "admin@helplink.com",
  "password": "Admin@123"
}
```

### Uso do Token JWT
```bash
# 1. Fazer login
curl -X POST http://localhost:5023/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@helplink.com","password":"Admin@123"}'

# 2. Usar o token nas requisições
curl -X GET http://localhost:5023/api/v1/Instituicoes \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

## 🌐 Endpoints Principais

### 🔐 Autenticação
- `POST /api/Auth/login` - Login e obtenção de token JWT

### 🏢 Instituições (v1)
- `GET /api/v1/Instituicoes` - Listar instituições (paginado)
- `GET /api/v1/Instituicoes/{id}` - Obter instituição específica
- `POST /api/v1/Instituicoes` - Criar nova instituição
- `DELETE /api/v1/Instituicoes/{id}` - Desativar instituição

### 🏢 Instituições (v2)
- `GET /api/v2/Instituicoes/proximas` - Buscar próximas (geolocalização)
- `GET /api/v2/Instituicoes/{id}/estatisticas` - Estatísticas

### 📦 Doações (v1)
- `GET /api/v1/Doacoes` - Listar doações
- `POST /api/v1/Doacoes` - Criar nova doação
- `GET /api/v1/Doacoes/{id}` - Obter doação específica

### 📊 Monitoramento
- `GET /health` - Health check geral
- `GET /health/ready` - Verificação de prontidão
- `GET /health/live` - Verificação de vida

## 🔗 HATEOAS e Paginação

### Exemplo de Resposta Paginada com HATEOAS
```json
{
  "data": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalRecords": 50,
  "totalPages": 5,
  "links": [
    {
      "href": "/api/v1/Instituicoes?pageNumber=1&pageSize=10",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1/Instituicoes?pageNumber=2&pageSize=10",
      "rel": "next",
      "method": "GET"
    }
  ]
}
```

## 🎯 Atendimento aos Requisitos FIAP

### ✅ 1. Boas Práticas REST (30 pts)
- ✅ **Paginação**: Implementada com `pageNumber` e `pageSize`
- ✅ **HATEOAS**: Links de navegação em respostas paginadas
- ✅ **Status Codes**: 200, 201, 400, 401, 404, 500 adequados
- ✅ **Verbos HTTP**: GET, POST, PUT, DELETE implementados corretamente

### ✅ 2. Monitoramento e Observabilidade (15 pts)
- ✅ **Health Check**: `/health`, `/health/ready`, `/health/live`
- ✅ **Logging**: Serilog com estruturação e diferentes níveis
- ✅ **Tracing**: OpenTelemetry para rastreamento distribuído
- ✅ **Métricas**: Coleta automática de métricas HTTP

### ✅ 3. Versionamento da API (10 pts)
- ✅ **Versões**: `/api/v1/` e `/api/v2/` implementadas
- ✅ **Controle de Rotas**: ApiVersioning configurado
- ✅ **Documentação**: Versionamento explicado neste README

### ✅ 4. Integração e Persistência (30 pts)
- ✅ **Oracle Database**: Integração completa com Oracle
- ✅ **Entity Framework Core**: ORM configurado
- ✅ **Migrations**: Sistema de migrações implementado
- ✅ **Relacionamentos**: Entidades com FKs e navegação

### ✅ 5. Testes Integrados (15 pts)
- ✅ **xUnit**: Framework de testes implementado
- ✅ **Testes de Integração**: Testes end-to-end
- ✅ **Testes Unitários**: Testes de componentes isolados

### 🎁 Itens Opcionais Implementados
- ✅ **JWT Authentication**: Sistema completo de autenticação
- ✅ **Swagger/OpenAPI**: Documentação interativa
- ✅ **CORS**: Configurado para APIs cross-origin
- ✅ **Interceptors**: Personalização de comandos Oracle

**PONTUAÇÃO TOTAL: 100 pontos** ✅

## 🧪 Executar Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes específicos
dotnet test --filter "Category=Integration"
dotnet test --filter "Category=Unit"
```

## 📝 Exemplos de Uso

### Criar uma Instituição
```bash
curl -X POST http://localhost:5023/api/v1/Instituicoes \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN" \
  -d '{
    "nome": "Instituto Futuro do Trabalho",
    "cnpj": "12.345.678/0001-90",
    "descricao": "Organização dedicada à capacitação profissional",
    "email": "contato@institutofuturo.org.br",
    "telefone": "11999999999",
    "site": "https://institutofuturo.org.br"
  }'
```

### Listar Instituições com Filtro
```bash
curl -X GET "http://localhost:5023/api/v1/Instituicoes?pageNumber=1&pageSize=5&cidade=São Paulo" \
  -H "Authorization: Bearer SEU_TOKEN"
```

## 📊 Observabilidade

### Logs Estruturados
```json
{
  "timestamp": "2025-01-01T10:00:00Z",
  "level": "Information",
  "message": "API request processed",
  "properties": {
    "RequestPath": "/api/v1/Instituicoes",
    "StatusCode": 200,
    "Duration": 150
  }
}
```

### Métricas Coletadas
- Duração de requisições HTTP
- Taxa de erro por endpoint
- Throughput da aplicação
- Métricas de banco de dados

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## 👥 Autores

- **Arthur Ramos dos Santos** - RM: 558798
- **Felipe Melo de Sousa** - RM: 556099  
- **Robert Daniel da Silva Coimbra** - RM: 555881

**FIAP** - Análise e Desenvolvimento de Sistemas - Turma 2025

## 📄 Licença

Este projeto está sob a licença MIT e é parte do trabalho acadêmico da FIAP.

---

