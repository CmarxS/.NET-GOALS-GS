# ?? Deploy no Render - Guia Completo

## ?? Pré-requisitos

- Conta no [Render](https://render.com/) (gratuita)
- Repositório GitHub público
- Dockerfile criado (? já incluído no projeto)

---

## ?? Passo a Passo

### 1. Preparar o Repositório

#### a) Commit do Dockerfile
```bash
cd C:\Users\caio0\Desktop\.NET-GS\WebApplication1
git add Dockerfile .dockerignore
git commit -m "feat: Add Dockerfile for Render deployment"
git push
```

---

### 2. Configurar no Render

#### a) Criar Novo Web Service

1. Acesse: https://dashboard.render.com/
2. Clique em **"New +"** ? **"Web Service"**
3. Conecte seu repositório GitHub:
   - Autorize o Render a acessar sua conta GitHub
   - Selecione o repositório: **`.NET-GOALS-GS`**

#### b) Configurações do Service

| Campo | Valor |
|-------|-------|
| **Name** | `future-work-api` |
| **Region** | `Oregon (US West)` ou mais próximo |
| **Branch** | `main` |
| **Root Directory** | `WebApplication1` |
| **Environment** | `Docker` |
| **Instance Type** | `Free` |

#### c) Build Command
```bash
# Render detecta automaticamente o Dockerfile
# Deixe em branco ou use: docker build -t app .
```

#### d) Start Command
```bash
# Render inicia automaticamente via ENTRYPOINT do Dockerfile
# Deixe em branco
```

---

### 3. Configurar Variáveis de Ambiente

No painel do Render, vá em **"Environment"** e adicione:

#### Variáveis Obrigatórias

| Key | Value | Descrição |
|-----|-------|-----------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Ambiente de execução |
| `ASPNETCORE_URLS` | `http://+:8080` | URL de bind |
| `ConnectionStrings__DefaultConnection` | `User Id=rm555997;Password=090705;Data Source=oracle.fiap.com.br:1521/orcl;` | String de conexão Oracle |
| `ApiSettings__ApiKey` | `FiapGS2024SecureKey` | API Key |

**?? IMPORTANTE**: 
- Use `__` (dois underscores) para separar níveis em variáveis de ambiente
- Exemplo: `ConnectionStrings__DefaultConnection` = `appsettings.json ? ConnectionStrings:DefaultConnection`

---

### 4. Deploy

1. Clique em **"Create Web Service"**
2. Aguarde o build (pode levar 5-10 minutos na primeira vez)
3. A URL será algo como: `https://future-work-api.onrender.com`

---

## ?? Verificar Deploy

### Health Check
```bash
curl https://future-work-api.onrender.com/health
```

**Resposta esperada:**
```json
{
  "status": "Healthy",
  "checks": {
    "database": "Healthy"
  }
}
```

### Swagger UI
Acesse no navegador:
```
https://future-work-api.onrender.com
```

---

## ?? Configurações Adicionais (Opcional)

### Auto-Deploy
? Já configurado! Render faz deploy automático a cada push na branch `main`

### Custom Domain (Plano Pago)
1. Vá em **"Settings"** ? **"Custom Domain"**
2. Adicione seu domínio
3. Configure DNS conforme instruções

### Logs
Ver logs em tempo real:
```bash
# No dashboard do Render, clique em "Logs"
```

---

## ?? Troubleshooting

### Problema: Build falha

**Solução 1**: Verificar se o Dockerfile está no diretório correto
```bash
# Deve estar em: WebApplication1/Dockerfile
```

**Solução 2**: Verificar logs de build no Render

### Problema: Aplicação não inicia

**Solução 1**: Verificar variáveis de ambiente
- `ASPNETCORE_URLS` deve ser `http://+:8080`
- Render usa a porta 8080 internamente

**Solução 2**: Verificar logs de runtime

### Problema: Erro de conexão com Oracle

**Solução**: Verificar se a connection string está correta
```bash
# Formato correto:
User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/orcl;
```

### Problema: Health check falha

**Solução**: Verificar se `/health` está acessível
```bash
curl https://SEU-APP.onrender.com/health
```

---

## ?? Plano Free - Limitações

### Render Free Tier
- ? **750 horas/mês** de runtime
- ? **512 MB RAM**
- ? **0.1 CPU**
- ? **SSL grátis**
- ?? **App dorme após 15min de inatividade**
- ?? **Cold start: ~30s** (primeira requisição após dormir)

### Dicas para Free Tier
1. **Manter app acordado**: Use serviços como [UptimeRobot](https://uptimerobot.com/) para fazer ping a cada 10 minutos
2. **Otimizar imagem**: Dockerfile já está otimizado com multi-stage build
3. **Cache de pacotes**: NuGet packages são cacheados entre builds

---

## ?? Monitoramento

### Métricas Disponíveis (Render Dashboard)
- CPU Usage
- Memory Usage
- Network
- Disk
- Response Time

### Alertas (Plano Pago)
- Email notifications
- Slack integration
- PagerDuty

---

## ?? CI/CD Pipeline

```
???????????????     ???????????????     ???????????????
?   Commit ?????>?  GitHub     ?????>?   Render    ?
?   & Push    ?     ?  Push     ?     ?   Deploy    ?
???????????????     ???????????????   ???????????????
```

**Fluxo automático:**
1. Desenvolvedor faz push para `main`
2. GitHub notifica Render via webhook
3. Render faz build da imagem Docker
4. Deploy da nova versão
5. Health check automático

---

## ?? Exemplo de Request

### Listar Usuários
```bash
curl -X GET "https://future-work-api.onrender.com/api/v1/users?pageNumber=1&pageSize=10" \
  -H "X-API-Key: FiapGS2024SecureKey"
```

### Criar Usuário
```bash
curl -X POST "https://future-work-api.onrender.com/api/v1/users" \
  -H "Content-Type: application/json" \
  -H "X-API-Key: FiapGS2024SecureKey" \
  -d '{
    "nome": "João Silva",
    "email": "joao@email.com",
    "senha": "senha123",
    "role": "USER"
  }'
```

---

## ?? Segurança

### Boas Práticas no Render

1. **Nunca commitar secrets no código**
   - Use variáveis de ambiente
   - Senha do Oracle deve estar apenas nas Environment Variables

2. **HTTPS automático**
   - Render fornece SSL/TLS gratuito
   - Todas as requisições são HTTPS

3. **API Key**
   - Configure via Environment Variables
   - Não hardcode no código

4. **Connection String**
   - Use Environment Variables
   - Formato: `ConnectionStrings__DefaultConnection`

---

## ?? Recursos Adicionais

### Documentação Oficial
- [Render Docs](https://render.com/docs)
- [Docker Deploy](https://render.com/docs/docker)
- [.NET on Render](https://render.com/docs/deploy-dotnet)

### Comunidade
- [Render Community](https://community.render.com/)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/render)

---

## ? Checklist Final

Antes de fazer deploy:

- [ ] Dockerfile criado
- [ ] .dockerignore criado
- [ ] Arquivos commitados e pushed
- [ ] Conta no Render criada
- [ ] Repositório conectado
- [ ] Variáveis de ambiente configuradas
- [ ] Deploy iniciado
- [ ] Health check passou
- [ ] Swagger acessível
- [ ] API Key funcionando
- [ ] Endpoints testados

---

## ?? Deploy Concluído!

Sua API está no ar em:
```
https://SEU-APP.onrender.com
```

**Próximos passos:**
1. Testar todos os endpoints
2. Configurar monitoramento
3. Documentar URL no README
4. Compartilhar com o time

---

<div align="center">

**Desenvolvido para Global Solution - FIAP 2024**  
**Deploy**: Render Free Tier  
**Versão**: 1.0.0

</div>
