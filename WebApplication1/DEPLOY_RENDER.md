# ?? Deploy no Render - Guia Completo

## ?? Pr�-requisitos

- Conta no [Render](https://render.com/) (gratuita)
- Reposit�rio GitHub p�blico
- Dockerfile criado (? j� inclu�do no projeto)

---

## ?? Passo a Passo

### 1. Preparar o Reposit�rio

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
3. Conecte seu reposit�rio GitHub:
   - Autorize o Render a acessar sua conta GitHub
   - Selecione o reposit�rio: **`.NET-GOALS-GS`**

#### b) Configura��es do Service

| Campo | Valor |
|-------|-------|
| **Name** | `future-work-api` |
| **Region** | `Oregon (US West)` ou mais pr�ximo |
| **Branch** | `main` |
| **Root Directory** | *(deixe em branco)* ⚠️ **IMPORTANTE** |
| **Environment** | `Docker` |
| **Instance Type** | `Free` |

⚠️ **IMPORTANTE**: 
- O **Root Directory** deve ficar **em branco** (ou usar `/`)
- O Dockerfile está na raiz do repositório
- O Dockerfile já aponta para a pasta `WebApplication1/` internamente

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

### 3. Configurar Vari�veis de Ambiente

No painel do Render, v� em **"Environment"** e adicione:

#### Vari�veis Obrigat�rias

| Key | Value | Descri��o |
|-----|-------|-----------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Ambiente de execu��o |
| `ASPNETCORE_URLS` | `http://+:8080` | URL de bind |
| `ConnectionStrings__DefaultConnection` | `User Id=rm555997;Password=090705;Data Source=oracle.fiap.com.br:1521/orcl;` | String de conex�o Oracle |
| `ApiSettings__ApiKey` | `FiapGS2024SecureKey` | API Key |

**?? IMPORTANTE**: 
- Use `__` (dois underscores) para separar n�veis em vari�veis de ambiente
- Exemplo: `ConnectionStrings__DefaultConnection` = `appsettings.json ? ConnectionStrings:DefaultConnection`

---

### 4. Deploy

1. Clique em **"Create Web Service"**
2. Aguarde o build (pode levar 5-10 minutos na primeira vez)
3. A URL ser� algo como: `https://future-work-api.onrender.com`

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

## ?? Configura��es Adicionais (Opcional)

### Auto-Deploy
? J� configurado! Render faz deploy autom�tico a cada push na branch `main`

### Custom Domain (Plano Pago)
1. V� em **"Settings"** ? **"Custom Domain"**
2. Adicione seu dom�nio
3. Configure DNS conforme instru��es

### Logs
Ver logs em tempo real:
```bash
# No dashboard do Render, clique em "Logs"
```

---

## ?? Troubleshooting

### Problema: Build falha

**Solu��o 1**: Verificar se o Dockerfile est� no diret�rio correto
```bash
# Deve estar em: WebApplication1/Dockerfile
```

**Solu��o 2**: Verificar logs de build no Render

### Problema: Aplica��o n�o inicia

**Solu��o 1**: Verificar vari�veis de ambiente
- `ASPNETCORE_URLS` deve ser `http://+:8080`
- Render usa a porta 8080 internamente

**Solu��o 2**: Verificar logs de runtime

### Problema: Erro de conex�o com Oracle

**Solu��o**: Verificar se a connection string est� correta
```bash
# Formato correto:
User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/orcl;
```

### Problema: Health check falha

**Solu��o**: Verificar se `/health` est� acess�vel
```bash
curl https://SEU-APP.onrender.com/health
```

---

## ?? Plano Free - Limita��es

### Render Free Tier
- ? **750 horas/m�s** de runtime
- ? **512 MB RAM**
- ? **0.1 CPU**
- ? **SSL gr�tis**
- ?? **App dorme ap�s 15min de inatividade**
- ?? **Cold start: ~30s** (primeira requisi��o ap�s dormir)

### Dicas para Free Tier
1. **Manter app acordado**: Use servi�os como [UptimeRobot](https://uptimerobot.com/) para fazer ping a cada 10 minutos
2. **Otimizar imagem**: Dockerfile j� est� otimizado com multi-stage build
3. **Cache de pacotes**: NuGet packages s�o cacheados entre builds

---

## ?? Monitoramento

### M�tricas Dispon�veis (Render Dashboard)
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

**Fluxo autom�tico:**
1. Desenvolvedor faz push para `main`
2. GitHub notifica Render via webhook
3. Render faz build da imagem Docker
4. Deploy da nova vers�o
5. Health check autom�tico

---

## ?? Exemplo de Request

### Listar Usu�rios
```bash
curl -X GET "https://future-work-api.onrender.com/api/v1/users?pageNumber=1&pageSize=10" \
  -H "X-API-Key: FiapGS2024SecureKey"
```

### Criar Usu�rio
```bash
curl -X POST "https://future-work-api.onrender.com/api/v1/users" \
  -H "Content-Type: application/json" \
  -H "X-API-Key: FiapGS2024SecureKey" \
  -d '{
    "nome": "Jo�o Silva",
    "email": "joao@email.com",
    "senha": "senha123",
    "role": "USER"
  }'
```

---

## ?? Seguran�a

### Boas Pr�ticas no Render

1. **Nunca commitar secrets no c�digo**
   - Use vari�veis de ambiente
   - Senha do Oracle deve estar apenas nas Environment Variables

2. **HTTPS autom�tico**
   - Render fornece SSL/TLS gratuito
   - Todas as requisi��es s�o HTTPS

3. **API Key**
   - Configure via Environment Variables
   - N�o hardcode no c�digo

4. **Connection String**
   - Use Environment Variables
   - Formato: `ConnectionStrings__DefaultConnection`

---

## ?? Recursos Adicionais

### Documenta��o Oficial
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
- [ ] Reposit�rio conectado
- [ ] Vari�veis de ambiente configuradas
- [ ] Deploy iniciado
- [ ] Health check passou
- [ ] Swagger acess�vel
- [ ] API Key funcionando
- [ ] Endpoints testados

---

## ?? Deploy Conclu�do!

Sua API est� no ar em:
```
https://SEU-APP.onrender.com
```

**Pr�ximos passos:**
1. Testar todos os endpoints
2. Configurar monitoramento
3. Documentar URL no README
4. Compartilhar com o time

---

<div align="center">

**Desenvolvido para Global Solution - FIAP 2024**  
**Deploy**: Render Free Tier  
**Vers�o**: 1.0.0

</div>
