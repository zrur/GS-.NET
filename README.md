# HelpLink API

API REST completa para plataforma de doacoes com Observabilidade Total, Testes Integrados e Autenticacao JWT.

## Funcionalidades Implementadas

### Monitoramento e Observabilidade (15 pts)
- Health Check com multiplos endpoints (/health, /health/ready, /health/live)
- Logging estruturado com Serilog (console + arquivo)
- Tracing e Metricas com OpenTelemetry

### Testes Integrados (15 pts)
- 11 testes xUnit (100% passando)
- Testes de Controllers (Instituicoes e Doacoes)
- Testes de Health Check
- InMemory Database para testes

### Autenticacao JWT (Pontos Extras)
- JWT Bearer implementado e testado
- Swagger com autenticacao integrada
- Endpoints protegidos funcionando

### API RESTful Completa
- CRUD de Instituicoes
- CRUD de Doacoes
- Versionamento de API (V1 e V2)
- Paginacao
- Documentacao OpenAPI/Swagger

## Arquitetura
```
HelpLink/
├── HelpLink.Domain/          # Entidades do dominio
├── HelpLink.Application/     # DTOs, Services, Configuration
├── HelpLink.Infrastructure/  # DbContext, Repositories
├── HelpLink.API/            # Controllers, Program.cs
└── HelpLink.Tests/          # Testes Integrados
```

Clean Architecture com separacao de responsabilidades.

## Como Executar

### Pre-requisitos
- .NET 8 SDK
- Oracle Database (opcional)

### Executar a API
```bash
cd HelpLink.API
dotnet run --urls "http://localhost:5001"
```

### Acessar

- Swagger: http://localhost:5001/swagger
- Health Check: http://localhost:5001/health

### Credenciais de Teste
```json
{
  "email": "admin@helplink.com",
  "password": "Admin@123"
}
```

## Testes
```bash
dotnet test
```

Resultado: 11/11 testes passando

## Endpoints Principais

### Autenticacao
- POST /api/Auth/login - Login (retorna JWT)
- GET /api/Auth/me - Endpoint protegido (requer JWT)

### Instituicoes (V1)
- GET /api/v1/Instituicoes - Listar com paginacao
- POST /api/v1/Instituicoes - Criar nova instituicao
- GET /api/v1/Instituicoes/{id} - Buscar por ID
- PUT /api/v1/Instituicoes/{id} - Atualizar
- DELETE /api/v1/Instituicoes/{id} - Deletar

### Instituicoes (V2)
- GET /api/v2/Instituicoes/proximas - Buscar proximas (geolocalizacao)
- GET /api/v2/Instituicoes/{id}/estatisticas - Estatisticas da instituicao

### Doacoes
- GET /api/v1/Doacoes - Listar doacoes
- GET /api/v1/Doacoes/instituicao/{id} - Doacoes por instituicao

### Monitoramento
- GET /health - Health Check completo
- GET /health/ready - Readiness probe
- GET /health/live - Liveness probe

## Tecnologias

- .NET 8 - Framework principal
- ASP.NET Core Web API - API REST
- Entity Framework Core - ORM
- Oracle Database - Banco de dados
- JWT Bearer - Autenticacao e Autorizacao
- Serilog - Logging estruturado
- OpenTelemetry - Tracing e metricas distribuidas
- xUnit - Framework de testes
- Swagger/OpenAPI - Documentacao interativa
- Health Checks - Monitoramento de saude

## Pontuacao

Total: 30+ pontos

- Monitoramento e Observabilidade: 15 pts
- Testes Integrados: 15 pts
- Autenticacao JWT: Pontos Extras

## Testes Realizados
```bash
# Health Check
API is running: Healthy

# JWT Authentication
Login funcionando
Token gerado com sucesso
Endpoint protegido autenticado

# Endpoints
GET /api/v1/Instituicoes - OK
GET /api/v1/Doacoes - OK
POST /api/Auth/login - OK
GET /api/Auth/me - OK (com JWT)
```

## Autor

Arthur - RM558798
FIAP - Analise e Desenvolvimento de Sistemas
Turma 2025

## Licenca

Este projeto e parte do trabalho academico da FIAP.
