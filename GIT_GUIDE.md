# ?? Guia de Comandos Git - .NET GOALS

## ?? Comandos Iniciais (Já Executados)

### Configuração Inicial
```bash
cd C:\Users\caio0\Desktop\.NET-GS
git init
git remote add origin https://github.com/CmarxS/.NET-GOALS-GS.git
```

### Primeiro Commit
```bash
git add .
git commit -m "Initial commit: Future of Work API - Complete project"
git branch -M main
git push -u origin main
```

---

## ?? Comandos Úteis para o Dia a Dia

### Ver Status do Repositório
```bash
git status
```

### Ver Histórico de Commits
```bash
git log --oneline
git log --graph --oneline --all
```

### Ver Diferenças
```bash
# Ver mudanças não commitadas
git diff

# Ver mudanças em arquivo específico
git diff WebApplication1/Program.cs
```

---

## ?? Fazendo Mudanças

### Adicionar Arquivos
```bash
# Adicionar arquivo específico
git add WebApplication1/Program.cs

# Adicionar todos os arquivos modificados
git add .

# Adicionar apenas arquivos .cs
git add *.cs
```

### Fazer Commit
```bash
# Commit com mensagem curta
git commit -m "feat: Add new feature"

# Commit com mensagem detalhada
git commit -m "feat: Add user authentication" -m "Implemented JWT tokens and refresh mechanism"
```

### Push para GitHub
```bash
# Push para branch principal
git push

# Push forçado (cuidado!)
git push -f origin main
```

---

## ?? Sincronizando com GitHub

### Baixar Mudanças
```bash
# Pull (fetch + merge)
git pull

# Fetch sem merge
git fetch origin
```

### Ver Branches Remotas
```bash
git branch -r
git branch -a  # Locais e remotas
```

---

## ?? Trabalhando com Branches

### Criar Nova Branch
```bash
# Criar e mudar para nova branch
git checkout -b feature/nova-funcionalidade

# Ou usando comando moderno
git switch -c feature/nova-funcionalidade
```

### Listar Branches
```bash
git branch
```

### Mudar de Branch
```bash
git checkout main
git switch main  # Comando moderno
```

### Deletar Branch
```bash
# Deletar branch local
git branch -d feature/concluida

# Forçar deleção
git branch -D feature/nao-finalizada

# Deletar branch remota
git push origin --delete feature/antiga
```

---

## ?? Merge e Rebase

### Merge
```bash
# Merge de outra branch na atual
git merge feature/nova-funcionalidade

# Merge com mensagem customizada
git merge feature/nova-funcionalidade -m "Merge feature X"
```

### Rebase
```bash
# Rebase da branch atual
git rebase main

# Continuar rebase após resolver conflitos
git rebase --continue

# Abortar rebase
git rebase --abort
```

---

## ?? Desfazendo Mudanças

### Desfazer Mudanças Locais
```bash
# Descartar mudanças em arquivo específico
git checkout -- WebApplication1/Program.cs

# Descartar todas as mudanças
git reset --hard HEAD
```

### Desfazer Último Commit (mantendo mudanças)
```bash
git reset --soft HEAD~1
```

### Desfazer Último Commit (descartando mudanças)
```bash
git reset --hard HEAD~1
```

### Reverter Commit Específico
```bash
git revert <commit-hash>
```

---

## ??? Tags (Versões)

### Criar Tag
```bash
# Tag leve
git tag v1.0.0

# Tag anotada (recomendado)
git tag -a v1.0.0 -m "Versão 1.0.0 - Release inicial"
```

### Listar Tags
```bash
git tag
git tag -l "v1.*"
```

### Push de Tags
```bash
# Push de tag específica
git push origin v1.0.0

# Push de todas as tags
git push origin --tags
```

### Deletar Tag
```bash
# Local
git tag -d v1.0.0

# Remota
git push origin --delete v1.0.0
```

---

## ?? Pesquisa e Navegação

### Procurar em Commits
```bash
# Procurar por mensagem
git log --grep="authentication"

# Procurar por autor
git log --author="seu-nome"

# Procurar mudanças em arquivo
git log -- WebApplication1/Program.cs
```

### Ver Conteúdo de Commit
```bash
git show <commit-hash>
git show HEAD  # Último commit
```

### Ver Quem Mudou Cada Linha
```bash
git blame WebApplication1/Program.cs
```

---

## ?? Limpeza

### Limpar Arquivos Não Rastreados
```bash
# Ver o que será deletado
git clean -n

# Deletar arquivos não rastreados
git clean -f

# Deletar diretórios não rastreados
git clean -fd
```

### Atualizar .gitignore
```bash
# Depois de atualizar .gitignore
git rm -r --cached .
git add .
git commit -m "chore: Update .gitignore"
```

---

## ?? Configuração

### Configurar Nome e Email
```bash
git config --global user.name "Seu Nome"
git config --global user.email "seu-email@email.com"
```

### Ver Configurações
```bash
git config --list
git config user.name
git config user.email
```

### Configurar Editor Padrão
```bash
git config --global core.editor "code --wait"  # VS Code
```

---

## ?? Stash (Guardar Mudanças Temporariamente)

### Salvar Mudanças
```bash
# Stash básico
git stash

# Stash com mensagem
git stash save "WIP: trabalhando em autenticação"

# Incluir arquivos não rastreados
git stash -u
```

### Ver Stashes
```bash
git stash list
```

### Aplicar Stash
```bash
# Aplicar último stash
git stash apply

# Aplicar stash específico
git stash apply stash@{1}

# Aplicar e remover
git stash pop
```

### Deletar Stash
```bash
# Deletar último
git stash drop

# Deletar específico
git stash drop stash@{1}

# Deletar todos
git stash clear
```

---

## ?? Trabalhando com Remotes

### Ver Remotes
```bash
git remote -v
```

### Adicionar Remote
```bash
git remote add upstream https://github.com/outro-usuario/repo.git
```

### Remover Remote
```bash
git remote remove upstream
```

### Mudar URL do Remote
```bash
git remote set-url origin https://github.com/novo-usuario/novo-repo.git
```

---

## ?? Situações Comuns

### Conflitos de Merge
```bash
# 1. Identificar conflitos
git status

# 2. Editar arquivos manualmente
# Procure por <<<<<<, ======, >>>>>>

# 3. Marcar como resolvido
git add arquivo-resolvido.cs

# 4. Completar merge
git commit
```

### Push Rejeitado (branch desatualizada)
```bash
# Opção 1: Pull e merge
git pull
git push

# Opção 2: Pull com rebase
git pull --rebase
git push
```

### Arquivo Grande Commitado Por Engano
```bash
# Remover do último commit
git rm --cached arquivo-grande.zip
git commit --amend

# Push forçado (cuidado!)
git push -f
```

---

## ?? Convenções de Commit

### Tipos de Commit (Conventional Commits)
```
feat: Nova funcionalidade
fix: Correção de bug
docs: Mudanças na documentação
style: Formatação, espaços em branco
refactor: Refatoração de código
test: Adicionar/modificar testes
chore: Tarefas de manutenção
perf: Melhorias de performance
ci: Mudanças em CI/CD
build: Mudanças no build
```

### Exemplos
```bash
git commit -m "feat: Add user registration endpoint"
git commit -m "fix: Correct email validation regex"
git commit -m "docs: Update API documentation"
git commit -m "test: Add unit tests for UserService"
git commit -m "refactor: Simplify authentication logic"
```

---

## ?? Ajuda

### Comandos de Ajuda
```bash
# Ajuda geral
git help

# Ajuda de comando específico
git help commit
git commit --help
```

### Links Úteis
- [Git Documentation](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)
- [Conventional Commits](https://www.conventionalcommits.org/)

---

## ?? Seu Repositório

**URL**: https://github.com/CmarxS/.NET-GOALS-GS  
**Branch Principal**: `main`  
**Remote**: `origin`

### Status Atual
```bash
? Repositório inicializado
? Remote configurado
? Primeiro commit feito
? README na raiz adicionado
? Push para GitHub concluído
```

### Próximos Passos Sugeridos
```bash
# 1. Criar tag da versão 1.0.0
git tag -a v1.0.0 -m "Versão 1.0.0 - Release inicial"
git push origin v1.0.0

# 2. Criar branch para desenvolvimento
git checkout -b develop

# 3. Futuras features em branches separadas
git checkout -b feature/nome-da-feature
```

---

**?? Seu projeto está no GitHub e pronto para ser compartilhado!**
