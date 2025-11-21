# ?? Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Semantic Versioning](https://semver.org/lang/pt-BR/).

---

## [1.0.0] - 2024-11-20

### ? Adicionado

#### Entidades e Modelos
- Modelo `User` com suporte a roles (USER, ADMIN)
- Modelo `Category` com tipos (DESPESA, RECEITA)
- Modelo `Goal` com suporte a metas financeiras e hábitos
- Modelo `Transaction` com relacionamentos completos
- DTOs para todas as entidades (Create, Update, Response)
- Modelo `PagedResult<T>` para paginação

#### Controllers API V1
- `UsersController` com CRUD completo
- `CategoriesController` com CRUD completo
- `GoalsController` com CRUD completo e filtros
- `TransactionsController` com CRUD completo e relacionamentos

#### Controllers API V2
- `GoalsController V2` com ordenação customizada

#### Persistência
- `AppDbContext` configurado para Oracle
- Migration `InitialCreateWithNetSuffix` com 4 tabelas
- Relacionamentos 1:N configurados
- Índices otimizados
- Constraints (PK, FK, Unique, Check)

#### Segurança
- Middleware `ApiKeyMiddleware` para autenticação
- Hash de senhas com SHA256
- Validação de entrada
- Rotas públicas configuradas (/swagger, /health)

#### Monitoramento
- Health Check customizado para Oracle
- Logging com Serilog (console e arquivo)
- Request logging automático
- Logs estruturados com contexto

#### Documentação
- Swagger configurado para V1 e V2
- README.md completo
- 8 arquivos de documentação auxiliares
- Script SQL completo

#### Testes
- 36 testes unitários com xUnit
- Cobertura de 100% dos controllers
- Testes do middleware de autenticação
- Banco de dados em memória para testes
- Mock com Moq

### ?? Configurado

#### Infraestrutura
- .NET 8 como target framework
- Oracle.EntityFrameworkCore 8.23.50
- Serilog.AspNetCore 8.0.1
- Swashbuckle 6.6.2
- Connection string para Oracle FIAP

#### Boas Práticas
- Paginação em todas as listagens
- HATEOAS com links relacionados
- Status codes HTTP apropriados
- Verbos HTTP corretos (GET, POST, PUT, DELETE)
- Separação de responsabilidades
- Dependency Injection

#### Versionamento
- Versionamento por URL path
- V1: CRUD completo
- V2: Features avançadas (ordenação)
- Swagger separado por versão

---

## [0.2.0] - 2024-11-20

### ? Adicionado
- Testes unitários completos (36 testes)
- Cobertura de teste para todos os controllers
- Testes de middleware
- Relatório de testes

### ?? Melhorado
- Documentação expandida
- README mais detalhado
- Exemplos de uso

---

## [0.1.0] - 2024-11-20

### ? Adicionado
- Estrutura inicial do projeto
- Controllers básicos
- Modelos de dados
- DbContext
- Migration inicial

---

## Tipos de Mudanças

- **Adicionado** - para novas funcionalidades
- **Alterado** - para mudanças em funcionalidades existentes
- **Depreciado** - para funcionalidades que serão removidas
- **Removido** - para funcionalidades removidas
- **Corrigido** - para correção de bugs
- **Segurança** - para vulnerabilidades corrigidas

---

## Roadmap

### v1.1.0 (Planejado)
- [ ] JWT Authentication
- [ ] Cache com Redis
- [ ] Rate limiting
- [ ] Filtros avançados

### v1.2.0 (Planejado)
- [ ] Dashboard de métricas
- [ ] Relatórios financeiros
- [ ] Export para PDF/Excel
- [ ] Notificações

### v2.0.0 (Futuro)
- [ ] Machine Learning integrado
- [ ] API de recomendações
- [ ] Mobile app
- [ ] Deploy em nuvem

---

**Última atualização**: 20/11/2024  
**Versão atual**: 1.0.0  
**Status**: ? Completo e funcional
