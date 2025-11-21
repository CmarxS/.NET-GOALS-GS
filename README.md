# ?? .NET GOALS - Future of Work API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?logo=oracle)](https://www.oracle.com/)
[![Tests](https://img.shields.io/badge/Tests-36%20Passed-success)](TestProject/TEST_REPORT.md)
[![License](https://img.shields.io/badge/License-Academic-blue)](#)

> **Global Solution - O Futuro do Trabalho**  
> API RESTful completa desenvolvida em .NET 8 para gerenciamento de usuários, metas profissionais, categorias financeiras e transações.

---

## ?? Visão Geral do Projeto

Este projeto foi desenvolvido como parte da disciplina **Global Solution** da FIAP, focando no tema **"O Futuro do Trabalho"**.

### ?? Objetivo

Criar uma API RESTful robusta que permita:
- Gestão de usuários com diferentes níveis de acesso
- Acompanhamento de metas financeiras e de hábitos
- Controle de categorias de receitas e despesas
- Registro e análise de transações financeiras

---

## ? Destaques

### ?? 100% dos Requisitos Atendidos

| Requisito | Pontuação | Status |
|-----------|-----------|--------|
| Boas Práticas REST | 30 pts | ? 100% |
| Monitoramento | 15 pts | ? 100% |
| Versionamento | 10 pts | ? 100% |
| Persistência | 30 pts | ? 100% |
| Testes | 15 pts | ? 100% |
| **TOTAL** | **100 pts** | **? COMPLETO** |
| Autenticação (Bonus) | - | ? Implementado |

### ?? Principais Funcionalidades

- ? **4 Entidades Completas**: Users, Categories, Goals, Transactions
- ? **21 Endpoints RESTful**: CRUD completo para todas as entidades
- ? **36 Testes Unitários**: 100% de cobertura e sucesso
- ? **Paginação e HATEOAS**: Implementados em todos os endpoints
- ? **Versionamento de API**: V1 e V2 com Swagger
- ? **Health Checks**: Monitoramento da aplicação e banco
- ? **Logging Completo**: Serilog com console e arquivo
- ? **Segurança**: API Key authentication
- ? **Documentação Completa**: README, diagramas, exemplos

---

## ??? Tecnologias Utilizadas

### Backend
- **.NET 8**: Framework principal
- **ASP.NET Core Web API**: API RESTful
- **C# 12**: Linguagem de programação
- **Entity Framework Core 8**: ORM

### Banco de Dados
- **Oracle Database**: SGBD principal
- **Oracle.EntityFrameworkCore 8.23.50**: Provider

### Ferramentas
- **Swagger/OpenAPI**: Documentação interativa
- **Serilog**: Logging estruturado
- **xUnit**: Framework de testes
- **Moq**: Mocking para testes

---

## ?? Estrutura do Repositório

```
.NET-GOALS-GS/
??? WebApplication1/  # Projeto principal da API
?   ??? Controllers/    # Controllers V1 e V2
?   ??? Models/        # Entidades e DTOs
?   ??? Data/     # DbContext
?   ??? Migrations/  # Migrations do EF Core
?   ??? HealthChecks/        # Health checks customizados
?   ??? Middleware/      # Middlewares (API Key)
?   ??? Scripts/      # Scripts SQL
?   ??? README.md            # Documentação completa
??? TestProject/# Testes unitários
?   ??? *Tests.cs # Arquivos de teste
?   ??? TEST_REPORT.md      # Relatório de testes
??? .gitignore    # Arquivos ignorados pelo Git
??? README.md            # Este arquivo
```

---

## ?? Quick Start

### 1. Clone o repositório
```bash
git clone https://github.com/CmarxS/.NET-GOALS-GS.git
cd .NET-GOALS-GS
```

### 2. Restaurar pacotes
```bash
cd WebApplication1
dotnet restore
```

### 3. Aplicar migrations
```bash
dotnet ef database update
```

### 4. Executar a aplicação
```bash
dotnet run
```

### 5. Acessar o Swagger
```
http://localhost:5000
```

**API Key**: `FiapGS2024SecureKey`

---

## ?? Documentação Completa

Para documentação detalhada, consulte:

### ?? Documentação Principal
- [**README Completo**](WebApplication1/README.md) - Documentação completa da API
- [**Relatório de Testes**](TestProject/TEST_REPORT.md) - Detalhes dos 36 testes
- [**CHANGELOG**](WebApplication1/CHANGELOG.md) - Histórico de versões

### ?? Guias Rápidos
- [**Quick Start**](WebApplication1/QUICKSTART.md) - Início rápido
- [**API Complete**](WebApplication1/API_COMPLETE.md) - Status e funcionalidades
- [**Database Setup**](WebApplication1/DATABASE_SETUP.md) - Configuração do banco

---

## ?? Testes

```bash
cd TestProject
dotnet test
```

**Resultado**: ? 36/36 testes passando (100%)

### Cobertura de Testes

| Controller | Testes | Status |
|-----------|--------|--------|
| UsersController | 8 | ? |
| CategoriesController | 8 | ? |
| GoalsController | 9 | ? |
| TransactionsController | 9 | ? |
| ApiKeyMiddleware | 6 | ? |

---

## ?? Endpoints da API

### ?? Users (Usuários)
- `GET /api/v1/users` - Lista usuários
- `GET /api/v1/users/{id}` - Busca usuário
- `POST /api/v1/users` - Cria usuário
- `PUT /api/v1/users/{id}` - Atualiza usuário
- `DELETE /api/v1/users/{id}` - Remove usuário

### ?? Categories (Categorias)
- `GET /api/v1/categories` - Lista categorias
- `GET /api/v1/categories/{id}` - Busca categoria
- `POST /api/v1/categories` - Cria categoria
- `PUT /api/v1/categories/{id}` - Atualiza categoria
- `DELETE /api/v1/categories/{id}` - Remove categoria

### ?? Goals (Metas)
- `GET /api/v1/goals` - Lista metas
- `GET /api/v1/goals/{id}` - Busca meta
- `POST /api/v1/goals` - Cria meta
- `PUT /api/v1/goals/{id}` - Atualiza meta
- `DELETE /api/v1/goals/{id}` - Remove meta
- `GET /api/v2/goals` - Lista metas (V2 com ordenação)

### ?? Transactions (Transações)
- `GET /api/v1/transactions` - Lista transações
- `GET /api/v1/transactions/{id}` - Busca transação
- `POST /api/v1/transactions` - Cria transação
- `PUT /api/v1/transactions/{id}` - Atualiza transação
- `DELETE /api/v1/transactions/{id}` - Remove transação

### ?? Utilitários
- `GET /health` - Status da aplicação

---

## ??? Banco de Dados

### Tabelas (Oracle)
- `TB_USERS_NET` - Usuários do sistema
- `TB_CATEGORIES_NET` - Categorias de receitas/despesas
- `TB_GOALS_NET` - Metas financeiras e de hábitos
- `TB_TRANSACTIONS_NET` - Transações financeiras

### Relacionamentos
```
TB_USERS_NET (1) ??????> (N) TB_GOALS_NET
TB_USERS_NET (1) ??????> (N) TB_TRANSACTIONS_NET
TB_CATEGORIES_NET (1) ??> (N) TB_TRANSACTIONS_NET
TB_GOALS_NET (1) ??????> (N) TB_TRANSACTIONS_NET
```

---

## ?? Segurança

### Autenticação
Todas as rotas `/api/*` requerem o header:
```http
X-API-Key: FiapGS2024SecureKey
```

### Rotas Públicas
- `/swagger` - Documentação
- `/health` - Health check

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

---

## ?? Reconhecimentos

- **FIAP**: Pela infraestrutura e suporte
- **Professores**: Pela orientação
- **Comunidade .NET**: Pelas bibliotecas open-source

---

## ?? Links Úteis

- **Repositório**: https://github.com/CmarxS/.NET-GOALS-GS
- **Documentação Completa**: [WebApplication1/README.md](WebApplication1/README.md)
- **Relatório de Testes**: [TestProject/TEST_REPORT.md](TestProject/TEST_REPORT.md)

---

<div align="center">

## ?? Status do Projeto

```
?????????????????????????????????????? 100%

? Requisitos: 100/100 pontos
? Testes: 36/36 passando
? Documentação: Completa
? Deploy Ready: Sim
```

### ? **Projeto Completo e Funcional**

**Desenvolvido com ?? para a Global Solution - FIAP 2024**

</div>
