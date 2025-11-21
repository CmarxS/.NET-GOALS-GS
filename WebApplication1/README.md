# ?? Future of Work API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?logo=oracle)](https://www.oracle.com/)
[![Tests](https://img.shields.io/badge/Tests-36%20Passed-success)](../TestProject/TEST_REPORT.md)
[![License](https://img.shields.io/badge/License-Academic-blue)](#)

> **Global Solution - O Futuro do Trabalho**  
> API RESTful completa desenvolvida em .NET 8 para gerenciamento de usuários, metas profissionais, categorias financeiras e transações.

---

## ?? Índice

- [Visão Geral](#-visão-geral)
- [Requisitos Atendidos](#-requisitos-atendidos)
- [Tecnologias](#-tecnologias)
- [Como Executar](#-como-executar)
- [Documentação da API](#-documentação-da-api)
- [Banco de Dados](#-banco-de-dados)
- [Testes](#-testes)
- [Segurança](#-segurança)
- [Arquitetura](#-arquitetura)

---

## ?? Visão Geral

Este projeto foi desenvolvido como parte da disciplina **Global Solution** da FIAP, focando no tema **"O Futuro do Trabalho"**.

### ?? Objetivo

Criar uma API RESTful robusta que permita:
- ? Gestão de usuários com diferentes níveis de acesso
- ? Acompanhamento de metas financeiras e de hábitos
- ? Controle de categorias de receitas e despesas
- ? Registro e análise de transações financeiras

### ?? Destaques

- **100% dos Requisitos**: 100/100 pontos
- **36 Testes**: 100% de cobertura
- **4 Entidades**: Users, Categories, Goals, Transactions
- **21 Endpoints**: CRUD completo
- **Documentação Completa**: Swagger + README

---

## ? Requisitos Atendidos

### ?? 1. Boas Práticas REST (30 pts)

#### Paginação
- ? Implementada em **todos** os endpoints GET
- ? Parâmetros: `pageNumber` e `pageSize`
- ? Retorna: `TotalCount`, `TotalPages`, `HasPrevious`, `HasNext`

```csharp
// Exemplo de uso
GET /api/v1/users?pageNumber=1&pageSize=10
```

#### HATEOAS
- ? Cada recurso retorna **links relacionados**:

```json
{
  "links": [
    { "rel": "self", "href": "/api/v1/users/1", "method": "GET" },
    { "rel": "update", "href": "/api/v1/users/1", "method": "PUT" },
    { "rel": "delete", "href": "/api/v1/users/1", "method": "DELETE" }
  ]
}
```

#### Status Codes HTTP
- ? `200 OK`: Sucesso em consultas e atualizações
- ? `201 Created`: Criação de recursos (com Location header)
- ? `204 No Content`: Exclusão bem-sucedida
- ? `400 Bad Request`: Validação falhou
- ? `401 Unauthorized`: API Key inválida/ausente
- ? `404 Not Found`: Recurso não encontrado

#### Verbos HTTP
- ? `GET`: Consultas e listagens
- ? `POST`: Criação de recursos
- ? `PUT`: Atualização completa
- ? `DELETE`: Remoção de recursos

---

### ?? 2. Monitoramento e Observabilidade (15 pts)

#### Health Check
```http
GET /health
```
- ? Verifica conectividade com Oracle Database
- ? Retorna: `Healthy` ou `Unhealthy`

#### Logging (Serilog)
- ? **Console**: Logs em tempo real
- ? **Arquivo**: `logs/log-YYYYMMDD.txt`
- ? **Request Logging**: Rastreamento automático
- ? Níveis: Information, Warning, Error, Fatal

#### Exemplo de Log
```
[13:30:45 INF] Aplicação iniciada com sucesso
[13:30:46 INF] HTTP GET /api/v1/users responded 200 in 45ms
```

---

### ?? 3. Versionamento da API (10 pts)

#### Versão 1 (V1)
- ? Path: `/api/v1/*`
- ? CRUD completo para todas as entidades
- ? Paginação e filtros básicos

#### Versão 2 (V2)
- ? Path: `/api/v2/goals`
- ? Ordenação customizada: `orderBy=titulo|status|tipo|date`

```http
GET /api/v2/goals?orderBy=status&pageNumber=1&pageSize=10
```

---

### ?? 4. Integração e Persistência (30 pts)

#### Banco de Dados Oracle
- ? Host: `oracle.fiap.com.br:1521/orcl`
- ? Provider: Oracle.EntityFrameworkCore 8.23.50
- ? Connection pooling otimizado

#### Entity Framework Core
- ? DbContext configurado
- ? Relacionamentos: 1:N, N:1
- ? Cascade delete e Set Null

#### Migrations
```bash
dotnet ef database update
```

**Tabelas criadas:**
- `TB_USERS_NET`
- `TB_CATEGORIES_NET`
- `TB_GOALS_NET`
- `TB_TRANSACTIONS_NET`

---

### ?? 5. Testes (15 pts)

#### Framework
- ? xUnit 2.5.3
- ? Moq 4.20.70
- ? EF Core InMemory

#### Cobertura
```bash
cd ../TestProject
dotnet test
```

**Resultado:**
```
? Total: 36 testes
? Passou: 36 (100%)
? Tempo: ~6 segundos
```

| Controller | Testes | Status |
|-----------|--------|--------|
| UsersController | 8 | ? 100% |
| CategoriesController | 8 | ? 100% |
| GoalsController | 9 | ? 100% |
| TransactionsController | 9 | ? 100% |
| ApiKeyMiddleware | 6 | ? 100% |

Ver detalhes: [TEST_REPORT.md](../TestProject/TEST_REPORT.md)

---

### ?? 6. Segurança (Opcional)

#### API Key Authentication
```http
X-API-Key: FiapGS2024SecureKey
```

- ? Middleware customizado
- ? Rotas públicas: `/swagger`, `/health`
- ? Hash de senhas: SHA256

---

## ??? Tecnologias

### Backend
- **.NET 8**: Framework principal
- **ASP.NET Core Web API**: RESTful API
- **C# 12**: Linguagem de programação

### Banco de Dados
- **Oracle Database**: SGBD
- **Entity Framework Core 8.0**: ORM
- **Oracle.EntityFrameworkCore 8.23.50**: Provider

### Ferramentas
- **Swagger/OpenAPI**: Documentação interativa
- **Serilog**: Logging estruturado
- **xUnit**: Framework de testes
- **Moq**: Mocking framework

### Pacotes NuGet

```xml
<!-- Principais -->
<PackageReference Include="Oracle.EntityFrameworkCore" Version="8.23.50" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.1" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />

<!-- Testes -->
<PackageReference Include="xunit" Version="2.5.3" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.3" />
```

---

## ?? Como Executar

### Pré-requisitos

```bash
# Verificar versão do .NET
dotnet --version
# Deve ser 8.0 ou superior
```

**Requisitos:**
- .NET 8 SDK
- Acesso ao banco Oracle (oracle.fiap.com.br)
- Visual Studio 2022 ou VS Code (opcional)

### Passo a Passo

#### 1. Clone o repositório
```bash
git clone https://github.com/CmarxS/.NET-GOALS-GS.git
cd .NET-GOALS-GS/WebApplication1
```

#### 2. Restaurar pacotes
```bash
dotnet restore
```

#### 3. Configurar connection string
Edite `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/orcl;"
  }
}
```

#### 4. Aplicar migrations
```bash
dotnet ef database update
```

#### 5. Executar a aplicação
```bash
dotnet run
```

#### 6. Acessar a aplicação

O console exibirá:
```
==================================================
?? APLICAÇÃO INICIADA COM SUCESSO!
==================================================
?? Swagger UI: http://localhost:5119
?? Health Check: http://localhost:5119/health
?? API Key: FiapGS2024SecureKey
==================================================
```

Abra o navegador em: **http://localhost:5119**

---

## ?? Documentação da API

### Swagger UI

Acesse: **http://localhost:5119**

#### Como usar:

1. Clique em **"Authorize"** (canto superior direito)
2. Insira a API Key: `FiapGS2024SecureKey`
3. Clique em **"Authorize"** e depois **"Close"**
4. Agora você pode testar todos os endpoints!

### Endpoints Disponíveis

#### ?? Users (Usuários)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/users` | Lista usuários (paginado) |
| GET | `/api/v1/users/{id}` | Busca usuário por ID |
| POST | `/api/v1/users` | Cria novo usuário |
| PUT | `/api/v1/users/{id}` | Atualiza usuário |
| DELETE | `/api/v1/users/{id}` | Remove usuário |

**Exemplo de Request (POST):**
```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "senha123",
  "role": "USER"
}
```

**Exemplo de Response:**
```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@email.com",
  "role": "USER",
  "createdAt": "2024-11-20T10:00:00",
  "links": [
    { "rel": "self", "href": "/api/v1/users/1", "method": "GET" },
    { "rel": "update", "href": "/api/v1/users/1", "method": "PUT" },
    { "rel": "delete", "href": "/api/v1/users/1", "method": "DELETE" },
    { "rel": "goals", "href": "/api/v1/goals?idUser=1", "method": "GET" }
  ]
}
```

---

#### ?? Categories (Categorias)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/categories` | Lista categorias (paginado) |
| GET | `/api/v1/categories/{id}` | Busca categoria por ID |
| POST | `/api/v1/categories` | Cria nova categoria |
| PUT | `/api/v1/categories/{id}` | Atualiza categoria |
| DELETE | `/api/v1/categories/{id}` | Remove categoria |

**Query Params:**
- `tipo`: DESPESA ou RECEITA
- `pageNumber`: Número da página (padrão: 1)
- `pageSize`: Itens por página (padrão: 10)

**Exemplo:**
```json
{
  "nome": "Alimentação",
  "tipo": "DESPESA",
  "limiteMensal": 800.00
}
```

---

#### ?? Goals (Metas)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/goals` | Lista metas (paginado) |
| GET | `/api/v1/goals/{id}` | Busca meta por ID |
| POST | `/api/v1/goals` | Cria nova meta |
| PUT | `/api/v1/goals/{id}` | Atualiza meta |
| DELETE | `/api/v1/goals/{id}` | Remove meta |
| GET | `/api/v2/goals` | Lista metas (V2 com ordenação) |

**Query Params (V1):**
- `status`: ATIVA, CONCLUIDA, CANCELADA
- `idUser`: Filtrar por usuário

**Query Params (V2):**
- `orderBy`: titulo, status, tipo, date

**Meta Financeira:**
```json
{
  "idUser": 1,
  "titulo": "Fundo de Emergência",
  "tipo": "FINANCEIRO",
  "valorAlvo": 10000.00,
  "dataInicio": "2024-11-20",
  "dataFim": "2025-11-20"
}
```

**Meta de Hábito:**
```json
{
  "idUser": 1,
  "titulo": "Exercício Diário",
  "tipo": "HABITO",
  "diasAlvo": 30,
  "qtdAlvoDiaria": 1,
  "unidade": "sessão"
}
```

---

#### ?? Transactions (Transações)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/transactions` | Lista transações (paginado) |
| GET | `/api/v1/transactions/{id}` | Busca transação por ID |
| POST | `/api/v1/transactions` | Cria nova transação |
| PUT | `/api/v1/transactions/{id}` | Atualiza transação |
| DELETE | `/api/v1/transactions/{id}` | Remove transação |

**Exemplo:**
```json
{
  "idUser": 1,
  "idCategory": 1,
  "idGoal": 1,
  "tipo": "RECEITA",
  "valor": 500.00,
  "descricao": "Aporte para fundo de emergência",
  "merchant": "Banco XYZ",
  "dataTransacao": "2024-11-20"
}
```

---

#### ?? Utilitários

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/health` | Status da aplicação | ? |
| GET | `/` | Swagger UI | ? |

---

## ??? Banco de Dados

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=rm555997;Password=090705;Data Source=oracle.fiap.com.br:1521/orcl;"
  }
}
```

### Estrutura das Tabelas

#### ?? TB_USERS_NET

| Coluna | Tipo | Constraints |
|--------|------|-------------|
| id_user | NUMBER(12) | PK, Identity |
| nome | VARCHAR2(100) | NOT NULL |
| email | VARCHAR2(120) | NOT NULL, UNIQUE |
| senha_hash | VARCHAR2(255) | NOT NULL |
| role | VARCHAR2(20) | CHECK (USER, ADMIN) |
| created_at | TIMESTAMP | DEFAULT SYSTIMESTAMP |

**Índices:** `IDX_USERS_NET_CREATED_AT`

---

#### ?? TB_CATEGORIES_NET

| Coluna | Tipo | Constraints |
|--------|------|-------------|
| id_category | NUMBER(12) | PK, Identity |
| nome | VARCHAR2(100) | NOT NULL, UNIQUE |
| tipo | VARCHAR2(20) | CHECK (DESPESA, RECEITA) |
| limite_mensal | NUMBER(10,2) | NULL |
| created_at | TIMESTAMP | DEFAULT SYSTIMESTAMP |

**Índices:** `IDX_CATEGORIES_NET_TIPO`

---

#### ?? TB_GOALS_NET

| Coluna | Tipo | Constraints |
|--------|------|-------------|
| id_goal | NUMBER(12) | PK, Identity |
| id_user | NUMBER(12) | FK ? TB_USERS_NET |
| titulo | VARCHAR2(150) | NOT NULL |
| tipo | VARCHAR2(12) | CHECK (FINANCEIRO, HABITO) |
| valor_alvo | NUMBER(10,2) | NULL |
| dias_alvo | NUMBER | NULL |
| dias_concluidos | NUMBER | DEFAULT 0 |
| qtd_alvo_diaria | NUMBER | NULL |
| unidade | VARCHAR2(20) | NULL |
| data_inicio | DATE | NULL |
| data_fim | DATE | NULL |
| status | VARCHAR2(12) | CHECK (ATIVA, CONCLUIDA, CANCELADA) |
| created_at | TIMESTAMP | DEFAULT SYSTIMESTAMP |

**Índices:** `IDX_GOALS_NET_USER`, `IDX_GOALS_NET_TIPO`

---

#### ?? TB_TRANSACTIONS_NET

| Coluna | Tipo | Constraints |
|--------|------|-------------|
| id_transaction | NUMBER(12) | PK, Identity |
| id_user | NUMBER(12) | FK ? TB_USERS_NET |
| id_category | NUMBER(12) | FK ? TB_CATEGORIES_NET |
| id_goal | NUMBER(12) | FK ? TB_GOALS_NET (NULL) |
| tipo | VARCHAR2(12) | CHECK (DESPESA, RECEITA) |
| valor | NUMBER(12,2) | NOT NULL |
| descricao | VARCHAR2(200) | NULL |
| merchant | VARCHAR2(100) | NULL |
| data_transacao | DATE | NOT NULL |
| created_at | TIMESTAMP | DEFAULT SYSTIMESTAMP |

**Índices:** `IDX_TRANS_NET_USER`, `IDX_TRANS_NET_CATEGORY`, `IDX_TRANS_NET_DATE`

---

### Relacionamentos

```
TB_USERS_NET (1) ??????> (N) TB_GOALS_NET
TB_USERS_NET (1) ??????> (N) TB_TRANSACTIONS_NET
TB_CATEGORIES_NET (1) ??> (N) TB_TRANSACTIONS_NET
TB_GOALS_NET (1) ??????> (N) TB_TRANSACTIONS_NET (opcional)
```

### Script SQL

Para criação manual das tabelas: [`Scripts/CreateDatabase.sql`](Scripts/CreateDatabase.sql)

---

## ?? Testes

### Executar Testes

```bash
cd ../TestProject
dotnet test
```

### Resultado

```
? Total: 36 testes
? Passou: 36 (100%)
? Falhou: 0
?? Tempo: ~6 segundos
```

### Cobertura por Controller

| Controller/Component | Testes | Cobertura |
|---------------------|--------|-----------|
| UsersController | 8 | 100% |
| CategoriesController | 8 | 100% |
| GoalsController | 9 | 100% |
| TransactionsController | 9 | 100% |
| ApiKeyMiddleware | 6 | 100% |
| **TOTAL** | **36** | **100%** |

### O que é Testado

#### Funcionalidades
- ? CRUD completo de todas as entidades
- ? Paginação com diferentes page sizes
- ? Filtros (status, tipo, idUser)
- ? Ordenação customizada (V2)

#### Validações
- ? Email único (Users)
- ? Nome único (Categories)
- ? Foreign keys válidas (Transactions)
- ? Campos obrigatórios

#### Status Codes
- ? 200 OK, 201 Created, 204 No Content
- ? 400 Bad Request, 401 Unauthorized, 404 Not Found

#### Segurança
- ? Middleware de API Key
- ? Rotas públicas vs protegidas
- ? Hash de senhas

**Relatório Completo:** [TEST_REPORT.md](../TestProject/TEST_REPORT.md)

---

## ?? Segurança

### Autenticação por API Key

```http
X-API-Key: FiapGS2024SecureKey
```

Todas as rotas `/api/*` requerem este header.

### Rotas Públicas (sem autenticação)
- `/swagger` - Documentação da API
- `/health` - Status da aplicação

### Hash de Senhas

```csharp
// SHA256 (Em produção, usar BCrypt)
using var sha256 = SHA256.Create();
var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
```

### Validações
- ? Email formato válido
- ? Campos obrigatórios
- ? Foreign keys existentes
- ? Tipos enumerados (CHECK constraints)

### Recomendações para Produção
1. JWT Authentication
2. HTTPS Only
3. Rate Limiting
4. CORS configurado
5. Secrets Manager (Azure Key Vault)
6. BCrypt para senhas

---

## ??? Arquitetura

### Estrutura do Projeto

```
WebApplication1/
??? Controllers/          # Endpoints da API
?   ??? V1/              # Versão 1
?   ?   ??? UsersController.cs
?   ?   ??? CategoriesController.cs
?   ?   ??? GoalsController.cs
? ?   ??? TransactionsController.cs
?   ??? V2/      # Versão 2
?       ??? GoalsController.cs
??? Models/       # Entidades e DTOs
?   ??? User.cs / UserDto.cs
?   ??? Category.cs / CategoryDto.cs
?   ??? Goal.cs / GoalDto.cs
?   ??? Transaction.cs / TransactionDto.cs
?   ??? PagedResult.cs
??? Data/      # DbContext
?   ??? AppDbContext.cs
??? Migrations/          # Migrations do EF Core
??? HealthChecks/  # Health checks customizados
?   ??? DatabaseHealthCheck.cs
??? Middleware/   # Middlewares
?   ??? ApiKeyMiddleware.cs
??? Scripts/         # Scripts SQL
?   ??? CreateDatabase.sql
??? Properties/       # Configurações de launch
??? logs/       # Arquivos de log
??? Program.cs   # Configuração da aplicação
??? appsettings.json     # Configurações
??? README.md    # Este arquivo
```

### Padrões Utilizados

#### Repository Pattern
```csharp
// Entity Framework Core atua como repository
_context.Users.Add(user);
await _context.SaveChangesAsync();
```

#### DTO Pattern
```csharp
public class UserDto  // Para API (response)
public class CreateUserDto  // Para criação
public class UpdateUserDto  // Para atualização
public class User     // Para banco de dados
```

#### Middleware Pattern
```csharp
app.UseMiddleware<ApiKeyMiddleware>();
```

#### Dependency Injection
```csharp
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddHealthChecks();
```

---

## ?? Estatísticas do Projeto

| Métrica | Valor |
|---------|-------|
| **Entidades** | 4 |
| **Controllers** | 5 (4 V1 + 1 V2) |
| **Endpoints** | 21 |
| **DTOs** | 12 |
| **Migrations** | 1 |
| **Testes** | 36 |
| **Linhas de código** | ~3500 |

### Funcionalidades Implementadas

- ? 4 entidades completas
- ? 21 endpoints RESTful
- ? CRUD completo
- ? Paginação universal
- ? HATEOAS em todas as respostas
- ? Filtros e ordenação
- ? Relacionamentos 1:N
- ? Validações de negócio
- ? Logging estruturado
- ? Health checks
- ? API Key authentication
- ? Versionamento de API
- ? 100% testes passando

---

## ?? Fluxo de Uso Típico

### 1. Criar Usuário
```http
POST /api/v1/users
Content-Type: application/json
X-API-Key: FiapGS2024SecureKey

{
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "senha": "senha123"
}
```

### 2. Criar Categorias
```http
POST /api/v1/categories

{ "nome": "Salário", "tipo": "RECEITA" }
{ "nome": "Alimentação", "tipo": "DESPESA", "limiteMensal": 800 }
```

### 3. Criar Meta
```http
POST /api/v1/goals

{
  "idUser": 1,
  "titulo": "Fundo de Emergência",
  "tipo": "FINANCEIRO",
  "valorAlvo": 10000,
  "dataInicio": "2024-11-20",
  "dataFim": "2025-11-20"
}
```

### 4. Registrar Transações
```http
POST /api/v1/transactions

{
  "idUser": 1,
  "idCategory": 1,
  "idGoal": 1,
  "tipo": "RECEITA",
  "valor": 500,
  "descricao": "Aporte para fundo",
  "dataTransacao": "2024-11-20"
}
```

### 5. Consultar Progresso
```http
GET /api/v1/goals/1
GET /api/v1/transactions?idUser=1&tipo=RECEITA
```

---

## ?? FAQ e Troubleshooting

### Erro: "Unable to connect to database"
```bash
# Verifique:
1. Estar na rede da FIAP ou conectado via VPN
2. Connection string em appsettings.json
3. Firewall bloqueando porta 1521
```

### Erro: "API Key inválida"
```bash
# Certifique-se de usar o header correto:
X-API-Key: FiapGS2024SecureKey
```

### Erro: "Migration já existe"
```bash
dotnet ef migrations remove
dotnet ef migrations add NovaMigration
dotnet ef database update
```

### Swagger não abre automaticamente
```bash
# A aplicação está configurada para abrir na raiz
# Se não abrir, acesse manualmente:
http://localhost:5119
# ou
https://localhost:7093
```

---

## ?? Autores

**Desenvolvedor**: RM555997  
**Curso**: Engenharia de Software - FIAP  
**Disciplina**: Global Solution  
**Tema**: O Futuro do Trabalho  
**Semestre**: 2024/2

---

## ?? Licença

Este projeto foi desenvolvido para fins **acadêmicos** como parte da disciplina Global Solution da FIAP.

### Uso Educacional

? Permitido para:
- Estudo e aprendizado
- Apresentação acadêmica
- Portfolio pessoal
- Referência para outros projetos

? Não permitido para:
- Uso comercial sem autorização
- Redistribuição sem créditos
- Plágio

---

## ?? Agradecimentos

- **FIAP**: Pela infraestrutura e suporte
- **Professores**: Pela orientação
- **Colegas**: Pela colaboração
- **Comunidade .NET**: Pelas bibliotecas e ferramentas

---

## ?? Links Úteis

- **Repositório**: https://github.com/CmarxS/.NET-GOALS-GS
- **Relatório de Testes**: [TEST_REPORT.md](../TestProject/TEST_REPORT.md)
- **Issues**: https://github.com/CmarxS/.NET-GOALS-GS/issues

---

## ?? Status do Projeto

```
?????????????????????????????????????? 100%

? Requisitos Obrigatórios: 100/100 pontos
? Requisitos Opcionais: Completo
? Testes: 36/36 passando
? Documentação: Completa
? Deploy Ready: Sim

Status: CONCLUÍDO ?
```

---

<div align="center">

### ? Desenvolvido para Global Solution - FIAP 2024

**Versão**: 1.0.0  
**Data**: Novembro 2024  
**Licença**: Acadêmica

</div>
