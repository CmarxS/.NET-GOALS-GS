# 🚀 GOALS API – Global Solution FIAP

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?logo=oracle)](https://www.oracle.com/)
[![Tests](https://img.shields.io/badge/Tests-36_Passed-success)](#)
[![License](https://img.shields.io/badge/License-Academic-blue)](#)

> API RESTful desenvolvida em **.NET 8** para o projeto **Global Solution – O Futuro do Trabalho (FIAP)**, com foco em gestão de usuários, categorias, metas e transações financeiras.  
---

# 👨‍💻 Autores

- **RM:** 555997 - Caio Marques
- **RM:** 556325 - Felipe Camargo
- **RM:** 558640 - Caio Amarante

---

# 📌 Índice

- [Visão Geral](#-visão-geral)
- [Funcionalidades](#-funcionalidades)
- [Tecnologias](#-tecnologias)
- [Como Executar](#-como-executar)
- [Documentação da API](#-documentação-da-api)
- [Banco de Dados](#-banco-de-dados)
- [Testes](#-testes)
- [Segurança](#-segurança)
- [Arquitetura](#-arquitetura)
- [Estatísticas](#-estatísticas)
- [Fluxo de Uso Típico](#-fluxo-de-uso-típico)
- [Licença](#-licença)

---

# 🌐 Visão Geral

API desenvolvida para o projeto **GOALS - Global Solution – FIAP**, com o tema **“O Futuro do Trabalho”**.  
A solução oferece:

- Gestão completa de usuários
- Categorias financeiras (despesas/receitas)
- Metas financeiras e metas de hábito
- Transações associadas a categorias e metas
- Testes completos e documentação avançada

---

# 🎯 Funcionalidades

- CRUD completo de **Users**, **Categories**, **Goals**, **Transactions**
- Paginação universal (`pageNumber`, `pageSize`)
- HATEOAS em todas as respostas
- Versionamento: `v1` e `v2`
- Autenticação via **API Key**
- Documentação via Swagger
- Health Check
- Logging com Serilog
- 36 testes automatizados (100% sucesso)

---

# 🛠 Tecnologias

### Backend
- .NET 8  
- ASP.NET Core Web API  
- C# 12  

### Banco de Dados
- Oracle Database  
- Entity Framework Core 8  
- Provider Oracle.EntityFrameworkCore  

### Outras Ferramentas
- Serilog  
- Swagger / OpenAPI  
- xUnit + Moq (testes)  

---

# ▶️ Como Executar

## 1. Clonar repositório
```bash
git clone https://github.com/CmarxS/.NET-GOALS-GS.git
cd .NET-GOALS-GS/WebApplication1
```

## 2. Restaurar pacotes
```bash
dotnet restore
```

## 3. Configurar connection string
No arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/orcl;"
  }
}
```

## 4. Aplicar migrations
```bash
dotnet ef database update
```

## 5. Rodar a aplicação
```bash
dotnet run
```

---

# 📘 Documentação da API

Swagger disponível em:

👉 **http://localhost:5119**

Para usar, adicione no header:

```
X-API-Key: FiapGS2024SecureKey
```

### Principais endpoints:

- `/api/v1/users`
- `/api/v1/categories`
- `/api/v1/goals`
- `/api/v2/goals`
- `/api/v1/transactions`

---

# 🗄 Banco de Dados

### Tabelas:

- `TB_USERS_NET`
- `TB_CATEGORIES_NET`
- `TB_GOALS_NET`
- `TB_TRANSACTIONS_NET`

Script completo: `Scripts/CreateDatabase.sql`

---

# 🧪 Testes

### Execução:
```bash
cd ../TestProject
dotnet test
```

### Resultado:
- **36 testes passando**
- Testam: CRUD, paginação, filtros, status codes, middleware, validações

---

# 🔒 Segurança

### API Key obrigatória:
```
X-API-Key: FiapGS2024SecureKey
```

### Rotas livres:
- `/swagger`
- `/health`

### Senhas:
- Hash SHA256 (modelo acadêmico)

---

# 🏗 Arquitetura

Estrutura simplificada:

```
Controllers (v1 e v2)
Models + DTOs
Data (DbContext)
Middleware (API Key)
HealthChecks
Scripts SQL
Logs
```

Padrões utilizados:

- DTO Pattern  
- Dependency Injection  
- Middleware Pattern  
- EF Core como repositório  

---

# 📊 Estatísticas

| Item | Quantidade |
|------|------------|
| Endpoints | 21 |
| Entidades | 4 |
| Controllers | 5 |
| DTOs | 12 |
| Testes | 36 |
| Linhas de código | ~3500 |

---

# 🔄 Fluxo de Uso Típico

1. Criar usuário  
2. Criar categorias  
3. Criar metas  
4. Registrar transações  
5. Consultar progresso  

---

# 📄 Licença

Projeto acadêmico — uso permitido para estudo e portfólio.  
Proibido uso comercial e plágio.

---

<div align="center">

### ✔ Projeto completo – Global Solution FIAP 2024  
**Versão:** 1.0.0

</div>
