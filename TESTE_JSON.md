# 📋 JSONs para Teste da API HelpLink

## 🔐 1. Login (obter token JWT)

```bash
curl -X POST http://localhost:5023/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@helplink.com",
    "password": "Admin@123"
  }'
```

**Resposta esperada:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6...",
  "email": "admin@helplink.com",
  "expiresAt": "2025-11-23T15:19:36Z"
}
```

## 🏢 2. Criar Instituição

```bash
curl -X POST http://localhost:5023/api/v1/Instituicoes \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {TOKEN_DO_LOGIN}" \
  -d '{
    "nome": "Instituto Futuro do Trabalho",
    "cnpj": "12.345.678/0001-90",
    "descricao": "Organização dedicada à capacitação profissional para o mercado de trabalho digital",
    "email": "contato@institutofuturo.org.br",
    "telefone": "11999999999",
    "site": "https://institutofuturo.org.br"
  }'
```

**Resposta esperada:**
```json
{
  "success": true,
  "message": "Instituição criada com sucesso",
  "data": {
    "id": 17,
    "nome": "Instituto Futuro do Trabalho",
    "cnpj": "12.345.678/0001-90",
    "verificada": 0,
    "endereco": null
  }
}
```

## 📦 3. Criar Doação

```bash
curl -X POST http://localhost:5023/api/v1/Doacoes \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {TOKEN}" \
  -d '{
    "usuarioId": 1,
    "instituicaoId": 16,
    "dataDoacao": "2025-01-15T10:00:00",
    "status": "Pendente",
    "observacoes": "Doação de equipamentos de informática para capacitação",
    "valorEstimado": 5000.00
  }'
```

## 📊 4. Health Check

```bash
curl -X GET http://localhost:5023/health
```

**Resposta esperada:**
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.008",
  "entries": {
    "self": {
      "status": "Healthy",
      "description": "API is running"
    }
  }
}
```

## 📄 5. Swagger Documentation

```bash
# Acesse no navegador:
http://localhost:5023/swagger
```

## 🎯 Teste Completo da API

### Passo 1: Health Check
```bash
curl http://localhost:5023/health
```

### Passo 2: Login
```bash
curl -X POST http://localhost:5023/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@helplink.com","password":"Admin@123"}'
```

### Passo 3: Usar o token retornado
```bash
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6..."

curl -X POST http://localhost:5023/api/v1/Instituicoes \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "nome": "ONG Tecnologia Social",
    "cnpj": "98.765.432/0001-10",
    "descricao": "Promovendo inclusão digital e capacitação tecnológica",
    "email": "contato@tecnologiasocial.org",
    "telefone": "11888888888",
    "site": "https://tecnologiasocial.org"
  }'
```

## ✅ Status de Testes Confirmados

| Funcionalidade | Status | Descrição |
|----------------|--------|-----------|
| JWT Login | ✅ | Funcionando perfeitamente |
| Criar Instituição | ✅ | POST funcionando com autenticação |
| Health Check | ✅ | Retorna status Healthy |
| Swagger | ✅ | Documentação interativa |
| Oracle Integration | ✅ | Persistindo dados no Oracle |
| Status Codes | ✅ | 200, 201, 401, 500 adequados |

## 🔍 Estrutura das Respostas

### Sucesso (201 Created)
```json
{
  "success": true,
  "message": "Instituição criada com sucesso",
  "data": { ... },
  "errors": [],
  "links": null
}
```

### Erro de Autenticação (401 Unauthorized)
```json
{
  "success": false,
  "message": "Token inválido",
  "data": null,
  "errors": [],
  "links": null
}
```

### Erro de Validação (400 Bad Request)
```json
{
  "success": false,
  "message": "Já existe uma instituição com este CNPJ",
  "data": null,
  "errors": [],
  "links": null
}
```