-- ===============================================
-- DDL.sql - MVP Futuro do Trabalho (.NET Version)
-- Tabelas: TB_USERS_NET, TB_CATEGORIES_NET, TB_GOALS_NET, TB_TRANSACTIONS_NET
-- ===============================================

-- ==============================
-- Tabela: TB_USERS_NET
-- ==============================
CREATE TABLE TB_USERS_NET (
  id_user     NUMBER(12) GENERATED ALWAYS AS IDENTITY
     CONSTRAINT PK_TB_USERS_NET PRIMARY KEY,
  nome        VARCHAR2(100)    NOT NULL,
  email     VARCHAR2(120)      NOT NULL,
  senha_hash  VARCHAR2(255)      NOT NULL,
  role        VARCHAR2(20)       DEFAULT 'USER' NOT NULL,
  created_at  TIMESTAMP  DEFAULT SYSTIMESTAMP
);

ALTER TABLE TB_USERS_NET
  ADD CONSTRAINT UQ_TB_USERS_NET_EMAIL UNIQUE (email);

ALTER TABLE TB_USERS_NET
  ADD CONSTRAINT CK_TB_USERS_NET_ROLE CHECK (role IN ('USER','ADMIN'));

CREATE INDEX IDX_USERS_NET_CREATED_AT ON TB_USERS_NET(created_at);

-- ==============================
-- Tabela: TB_CATEGORIES_NET
-- ==============================
CREATE TABLE TB_CATEGORIES_NET (
  id_category     NUMBER(12) GENERATED ALWAYS AS IDENTITY
           CONSTRAINT PK_TB_CATEGORIES_NET PRIMARY KEY,
  nome      VARCHAR2(100) NOT NULL,
  tipo      VARCHAR2(20)  NOT NULL, -- DESPESA | RECEITA
  limite_mensal   NUMBER(10,2),    -- opcional
  created_at      TIMESTAMP DEFAULT SYSTIMESTAMP
);

ALTER TABLE TB_CATEGORIES_NET
  ADD CONSTRAINT UQ_TB_CATEGORIES_NET_NOME UNIQUE (nome);

ALTER TABLE TB_CATEGORIES_NET
  ADD CONSTRAINT CK_TB_CATEGORIES_NET_TIPO CHECK (tipo IN ('DESPESA','RECEITA'));

CREATE INDEX IDX_CATEGORIES_NET_TIPO ON TB_CATEGORIES_NET(tipo);

-- ==============================
-- Tabela: TB_GOALS_NET
-- ==============================
CREATE TABLE TB_GOALS_NET (
  id_goal          NUMBER(12) GENERATED ALWAYS AS IDENTITY
          CONSTRAINT PK_TB_GOALS_NET PRIMARY KEY,
  id_user          NUMBER(12),   -- nullable
  titulo           VARCHAR2(150) NOT NULL,
  tipo             VARCHAR2(12)  NOT NULL,    -- FINANCEIRO | HABITO
  valor_alvo   NUMBER(10,2),              -- se FINANCEIRO
  dias_alvo        NUMBER,           -- se HABITO
  dias_concluidos  NUMBER DEFAULT 0,
  qtd_alvo_diaria  NUMBER,
  unidade    VARCHAR2(20),
  data_inicio      DATE,
data_fim         DATE,
  status   VARCHAR2(12) DEFAULT 'ATIVA',
  created_at       TIMESTAMP DEFAULT SYSTIMESTAMP
);

ALTER TABLE TB_GOALS_NET
  ADD CONSTRAINT FK_GOALS_NET_USER FOREIGN KEY (id_user)
  REFERENCES TB_USERS_NET(id_user) ON DELETE CASCADE;

ALTER TABLE TB_GOALS_NET
  ADD CONSTRAINT CK_TB_GOALS_NET_TIPO CHECK (tipo IN ('FINANCEIRO','HABITO'));

ALTER TABLE TB_GOALS_NET
  ADD CONSTRAINT CK_TB_GOALS_NET_STATUS CHECK (status IN ('ATIVA','CONCLUIDA','CANCELADA'));

CREATE INDEX IDX_GOALS_NET_USER ON TB_GOALS_NET(id_user);
CREATE INDEX IDX_GOALS_NET_TIPO ON TB_GOALS_NET(tipo);

-- ==============================
-- Tabela: TB_TRANSACTIONS_NET
-- ==============================
CREATE TABLE TB_TRANSACTIONS_NET (
  id_transaction  NUMBER(12) GENERATED ALWAYS AS IDENTITY
                  CONSTRAINT PK_TB_TRANSACTIONS_NET PRIMARY KEY,
  id_user         NUMBER(12)   NOT NULL,
  id_category     NUMBER(12)   NOT NULL,
  id_goal         NUMBER(12),        -- opcional (aporte para meta financeira)
  tipo       VARCHAR2(12) NOT NULL,   -- DESPESA | RECEITA
  valor           NUMBER(12,2) NOT NULL,
descricao       VARCHAR2(200),
  merchant    VARCHAR2(100),
  data_transacao  DATE         NOT NULL,
  created_at      TIMESTAMP    DEFAULT SYSTIMESTAMP
);

ALTER TABLE TB_TRANSACTIONS_NET
  ADD CONSTRAINT FK_TRANS_NET_USER FOREIGN KEY (id_user)
  REFERENCES TB_USERS_NET(id_user) ON DELETE CASCADE;

ALTER TABLE TB_TRANSACTIONS_NET
  ADD CONSTRAINT FK_TRANS_NET_CATEGORY FOREIGN KEY (id_category)
  REFERENCES TB_CATEGORIES_NET(id_category) ON DELETE CASCADE;

ALTER TABLE TB_TRANSACTIONS_NET
  ADD CONSTRAINT FK_TRANS_NET_GOAL FOREIGN KEY (id_goal)
  REFERENCES TB_GOALS_NET(id_goal) ON DELETE SET NULL;

ALTER TABLE TB_TRANSACTIONS_NET
  ADD CONSTRAINT CK_TB_TRANS_NET_TIPO CHECK (tipo IN ('DESPESA','RECEITA'));

CREATE INDEX IDX_TRANS_NET_USER ON TB_TRANSACTIONS_NET(id_user);
CREATE INDEX IDX_TRANS_NET_CATEGORY ON TB_TRANSACTIONS_NET(id_category);
CREATE INDEX IDX_TRANS_NET_DATE ON TB_TRANSACTIONS_NET(data_transacao);

-- ==============================
-- Dados de Exemplo
-- ==============================

-- Inserir usuário exemplo
INSERT INTO TB_USERS_NET (nome, email, senha_hash, role)
VALUES ('João Silva', 'joao.silva@email.com', 'HASH_EXEMPLO_123', 'USER');

-- Inserir categorias exemplo
INSERT INTO TB_CATEGORIES_NET (nome, tipo, limite_mensal)
VALUES ('Salário', 'RECEITA', NULL);

INSERT INTO TB_CATEGORIES_NET (nome, tipo, limite_mensal)
VALUES ('Alimentação', 'DESPESA', 800.00);

INSERT INTO TB_CATEGORIES_NET (nome, tipo, limite_mensal)
VALUES ('Transporte', 'DESPESA', 400.00);

INSERT INTO TB_CATEGORIES_NET (nome, tipo, limite_mensal)
VALUES ('Educação', 'DESPESA', 1000.00);

-- Inserir metas exemplo
INSERT INTO TB_GOALS_NET (id_user, titulo, tipo, valor_alvo, status, data_inicio, data_fim)
VALUES (1, 'Fundo de Emergência', 'FINANCEIRO', 10000.00, 'ATIVA', SYSDATE, ADD_MONTHS(SYSDATE, 12));

INSERT INTO TB_GOALS_NET (id_user, titulo, tipo, dias_alvo, qtd_alvo_diaria, unidade, status, data_inicio)
VALUES (1, 'Exercícios Diários', 'HABITO', 30, 1, 'sessão', 'ATIVA', SYSDATE);

INSERT INTO TB_GOALS_NET (id_user, titulo, tipo, valor_alvo, status, data_inicio, data_fim)
VALUES (1, 'Curso de .NET', 'FINANCEIRO', 2000.00, 'CONCLUIDA', ADD_MONTHS(SYSDATE, -6), ADD_MONTHS(SYSDATE, -1));

-- Inserir transações exemplo
INSERT INTO TB_TRANSACTIONS_NET (id_user, id_category, tipo, valor, descricao, merchant, data_transacao)
VALUES (1, 1, 'RECEITA', 5000.00, 'Salário mensal', 'Empresa XYZ', SYSDATE);

INSERT INTO TB_TRANSACTIONS_NET (id_user, id_category, tipo, valor, descricao, merchant, data_transacao)
VALUES (1, 2, 'DESPESA', 150.50, 'Compras no supermercado', 'Supermercado ABC', SYSDATE - 1);

INSERT INTO TB_TRANSACTIONS_NET (id_user, id_category, id_goal, tipo, valor, descricao, data_transacao)
VALUES (1, 1, 1, 'RECEITA', 500.00, 'Aporte para fundo de emergência', SYSDATE - 2);

COMMIT;

-- ==============================
-- Consultas Úteis
-- ==============================

-- Ver todos os usuários
SELECT * FROM TB_USERS_NET ORDER BY created_at DESC;

-- Ver todas as categorias
SELECT * FROM TB_CATEGORIES_NET ORDER BY tipo, nome;

-- Ver todas as metas
SELECT * FROM TB_GOALS_NET ORDER BY created_at DESC;

-- Ver todas as transações com informações relacionadas
SELECT 
    t.id_transaction,
    u.nome as usuario,
    c.nome as categoria,
    g.titulo as meta,
    t.tipo,
    t.valor,
    t.descricao,
    t.data_transacao
FROM TB_TRANSACTIONS_NET t
INNER JOIN TB_USERS_NET u ON t.id_user = u.id_user
INNER JOIN TB_CATEGORIES_NET c ON t.id_category = c.id_category
LEFT JOIN TB_GOALS_NET g ON t.id_goal = g.id_goal
ORDER BY t.data_transacao DESC;

-- Ver saldo total por usuário
SELECT 
    u.nome,
    SUM(CASE WHEN t.tipo = 'RECEITA' THEN t.valor ELSE 0 END) as receitas,
    SUM(CASE WHEN t.tipo = 'DESPESA' THEN t.valor ELSE 0 END) as despesas,
    SUM(CASE WHEN t.tipo = 'RECEITA' THEN t.valor ELSE -t.valor END) as saldo
FROM TB_USERS_NET u
LEFT JOIN TB_TRANSACTIONS_NET t ON u.id_user = t.id_user
GROUP BY u.nome;

-- Ver progresso das metas financeiras
SELECT 
    g.titulo,
    g.valor_alvo,
    COALESCE(SUM(t.valor), 0) as valor_atual,
    g.status
FROM TB_GOALS_NET g
LEFT JOIN TB_TRANSACTIONS_NET t ON g.id_goal = t.id_goal
WHERE g.tipo = 'FINANCEIRO'
GROUP BY g.id_goal, g.titulo, g.valor_alvo, g.status;

-- ==============================
-- Limpeza (se necessário)
-- ==============================

-- CUIDADO: Isso apaga TODAS as tabelas e dados!
-- Descomente apenas se precisar resetar tudo

/*
DROP TABLE TB_TRANSACTIONS_NET CASCADE CONSTRAINTS;
DROP TABLE TB_GOALS_NET CASCADE CONSTRAINTS;
DROP TABLE TB_CATEGORIES_NET CASCADE CONSTRAINTS;
DROP TABLE TB_USERS_NET CASCADE CONSTRAINTS;
*/
