-- =============================================
-- TalentBridge - Migration 0001
-- Tabelas base: Perfils, Funcionalidades, Operacoes
-- =============================================

-- Extensão para UUID
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- =============================================
-- Tabela: Perfils
-- =============================================
CREATE TABLE IF NOT EXISTS "Perfils" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Codigo" VARCHAR(50) NOT NULL UNIQUE,
    "Nome" VARCHAR(100) NOT NULL,
    "Descricao" VARCHAR(500) NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- Tabela: Funcionalidades
-- =============================================
CREATE TABLE IF NOT EXISTS "Funcionalidades" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Codigo" VARCHAR(100) NOT NULL UNIQUE,
    "Descricao" VARCHAR(500) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- Tabela: Operacoes
-- =============================================
CREATE TABLE IF NOT EXISTS "Operacoes" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "FuncionalidadeId" UUID NOT NULL,
    "Codigo" VARCHAR(100) NOT NULL UNIQUE,
    "Descricao" VARCHAR(500) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Operacoes_Funcionalidades" FOREIGN KEY ("FuncionalidadeId") 
        REFERENCES "Funcionalidades"("Id") ON DELETE CASCADE
);

-- =============================================
-- Tabela: PerfilFuncionalidade (N:N)
-- =============================================
CREATE TABLE IF NOT EXISTS "PerfilFuncionalidades" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PerfilId" UUID NOT NULL,
    "FuncionalidadeId" UUID NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_PerfilFunc_Perfil" FOREIGN KEY ("PerfilId") 
        REFERENCES "Perfils"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PerfilFunc_Funcionalidade" FOREIGN KEY ("FuncionalidadeId") 
        REFERENCES "Funcionalidades"("Id") ON DELETE CASCADE,
    CONSTRAINT "UQ_PerfilFuncionalidade" UNIQUE ("PerfilId", "FuncionalidadeId")
);

-- =============================================
-- Tabela: Usuarios (base TPH)
-- =============================================
CREATE TABLE IF NOT EXISTS "Usuarios" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Nome" VARCHAR(128) NOT NULL,
    "Email" VARCHAR(100) NOT NULL UNIQUE,
    "SenhaHash" VARCHAR(500) NOT NULL,
    "Telefone" VARCHAR(20) NULL,
    "Status" INTEGER NOT NULL DEFAULT 1, -- Ativo=1, Inativo=0, Pendente=2
    "OrigemCadastro" INTEGER NOT NULL DEFAULT 1, -- Web=1, WhatsApp=2, Outro=3
    "PerfilId" UUID NOT NULL,
    "Discriminator" VARCHAR(50) NOT NULL, -- Candidato, Gestor, Recrutador
    "RefreshToken" VARCHAR(500) NULL,
    "RefreshTokenExpiryTime" TIMESTAMP WITH TIME ZONE NULL,
    "UltimoLogin" TIMESTAMP WITH TIME ZONE NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Usuarios_Perfils" FOREIGN KEY ("PerfilId") 
        REFERENCES "Perfils"("Id")
);

-- Índices
CREATE INDEX IF NOT EXISTS "IX_Usuarios_Email" ON "Usuarios"("Email");
CREATE INDEX IF NOT EXISTS "IX_Usuarios_PerfilId" ON "Usuarios"("PerfilId");
CREATE INDEX IF NOT EXISTS "IX_Usuarios_Discriminator" ON "Usuarios"("Discriminator");

-- =============================================
-- Tabela: RedefinicaoSenha
-- =============================================
CREATE TABLE IF NOT EXISTS "RedefinicoesSenha" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UsuarioId" UUID NOT NULL,
    "Token" VARCHAR(500) NOT NULL,
    "DataExpiracao" TIMESTAMP WITH TIME ZONE NOT NULL,
    "Utilizado" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_RedefinicaoSenha_Usuario" FOREIGN KEY ("UsuarioId") 
        REFERENCES "Usuarios"("Id") ON DELETE CASCADE
);

-- =============================================
-- Tabela: Segmentos
-- =============================================
CREATE TABLE IF NOT EXISTS "Segmentos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Nome" VARCHAR(100) NOT NULL,
    "Descricao" VARCHAR(500) NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- Tabela: Empresas
-- =============================================
CREATE TABLE IF NOT EXISTS "Empresas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Nome" VARCHAR(200) NOT NULL,
    "CNPJ" VARCHAR(14) NOT NULL UNIQUE,
    "Email" VARCHAR(200) NOT NULL,
    "Telefone" VARCHAR(20) NOT NULL,
    "SegmentoId" UUID NOT NULL,
    "Missao" TEXT NULL,
    "Comentarios" TEXT NULL,
    "Historia" TEXT NULL,
    "Visao" TEXT NULL,
    "Valores" TEXT NULL,
    "Website" VARCHAR(500) NULL,
    "Whatsapp" VARCHAR(20) NULL,
    "Instagram" VARCHAR(200) NULL,
    "Iniciativas" TEXT NULL,
    
    -- Endereco (Value Object)
    "CEP" VARCHAR(8) NULL,
    "Rua" VARCHAR(200) NULL,
    "Numero" VARCHAR(10) NULL,
    "Bairro" VARCHAR(100) NULL,
    "Cidade" VARCHAR(100) NULL,
    "Estado" VARCHAR(2) NULL,
    "Complemento" VARCHAR(200) NULL,
    "Latitude" DOUBLE PRECISION NULL,
    "Longitude" DOUBLE PRECISION NULL,
    
    -- Configurações
    "DesativarNotificacoes" BOOLEAN DEFAULT FALSE,
    "ReceberNotificacoesCandidaturas" BOOLEAN DEFAULT TRUE,
    "EnviarEmailNovoLoginNavegador" BOOLEAN DEFAULT TRUE,
    "EnviarEmailAtividadeIncomum" BOOLEAN DEFAULT TRUE,
    
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    
    CONSTRAINT "FK_Empresas_Segmentos" FOREIGN KEY ("SegmentoId") 
        REFERENCES "Segmentos"("Id")
);

CREATE INDEX IF NOT EXISTS "IX_Empresas_CNPJ" ON "Empresas"("CNPJ");
CREATE INDEX IF NOT EXISTS "IX_Empresas_SegmentoId" ON "Empresas"("SegmentoId");

-- =============================================
-- Tabela: Dominios (Lookup Table)
-- =============================================
CREATE TABLE IF NOT EXISTS "Dominios" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Codigo" INTEGER NOT NULL,
    "Descricao" VARCHAR(200) NOT NULL,
    "Tipo" INTEGER NOT NULL,
    "Status" INTEGER NOT NULL DEFAULT 1, -- Ativo=1, Inativo=0
    "DataInativacao" TIMESTAMP WITH TIME ZONE NULL,
    "UsuarioInativacaoId" UUID NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "UQ_Dominio_Tipo_Codigo" UNIQUE ("Tipo", "Codigo")
);

CREATE INDEX IF NOT EXISTS "IX_Dominios_Tipo" ON "Dominios"("Tipo");
CREATE INDEX IF NOT EXISTS "IX_Dominios_Tipo_Codigo" ON "Dominios"("Tipo", "Codigo");

-- =============================================
-- SEED DATA: Perfis
-- =============================================
INSERT INTO "Perfils" ("Id", "Codigo", "Nome", "Descricao") VALUES
    ('a1b2c3d4-0001-4000-8000-000000000001', 'ADMIN', 'Administrador', 'Acesso total ao sistema'),
    ('a1b2c3d4-0002-4000-8000-000000000002', 'GESTOR_EMPRESA', 'Gestor de Empresa', 'Gerencia empresa, vagas e recrutadores'),
    ('a1b2c3d4-0003-4000-8000-000000000003', 'RECRUTADOR', 'Recrutador', 'Gerencia vagas e candidaturas'),
    ('a1b2c3d4-0004-4000-8000-000000000004', 'CANDIDATO', 'Candidato', 'Busca vagas e gerencia perfil')
ON CONFLICT ("Codigo") DO NOTHING;

-- =============================================
-- SEED DATA: Funcionalidades
-- =============================================
INSERT INTO "Funcionalidades" ("Id", "Codigo", "Descricao") VALUES
    ('b1c2d3e4-0001-4000-8000-000000000001', 'ADMIN_DADOS', 'Acesso aos dados administrativos'),
    ('b1c2d3e4-0002-4000-8000-000000000002', 'GESTOR_DADOS', 'Acesso aos dados do gestor'),
    ('b1c2d3e4-0003-4000-8000-000000000003', 'EMPRESA_DADOS', 'Acesso aos dados da empresa'),
    ('b1c2d3e4-0004-4000-8000-000000000004', 'RECRUTADOR_DADOS', 'Acesso aos dados do recrutador'),
    ('b1c2d3e4-0005-4000-8000-000000000005', 'CANDIDATO_DADOS', 'Acesso aos dados do candidato')
ON CONFLICT ("Codigo") DO NOTHING;

-- =============================================
-- SEED DATA: Operações
-- =============================================
INSERT INTO "Operacoes" ("Id", "FuncionalidadeId", "Codigo", "Descricao") VALUES
    -- Admin
    ('c1d2e3f4-0001-4000-8000-000000000001', 'b1c2d3e4-0001-4000-8000-000000000001', 'ADMIN_VISUALIZAR', 'Visualizar dados administrativos'),
    ('c1d2e3f4-0002-4000-8000-000000000002', 'b1c2d3e4-0001-4000-8000-000000000001', 'ADMIN_CRIAR', 'Criar registros administrativos'),
    ('c1d2e3f4-0003-4000-8000-000000000003', 'b1c2d3e4-0001-4000-8000-000000000001', 'ADMIN_ATUALIZAR', 'Atualizar registros administrativos'),
    ('c1d2e3f4-0004-4000-8000-000000000004', 'b1c2d3e4-0001-4000-8000-000000000001', 'ADMIN_DELETAR', 'Deletar registros administrativos'),
    -- Gestor
    ('c1d2e3f4-0005-4000-8000-000000000005', 'b1c2d3e4-0002-4000-8000-000000000002', 'GESTOR_VISUALIZAR', 'Visualizar dados do gestor'),
    ('c1d2e3f4-0006-4000-8000-000000000006', 'b1c2d3e4-0002-4000-8000-000000000002', 'GESTOR_ATUALIZAR', 'Atualizar dados do gestor'),
    -- Empresa
    ('c1d2e3f4-0007-4000-8000-000000000007', 'b1c2d3e4-0003-4000-8000-000000000003', 'EMPRESA_VISUALIZAR', 'Visualizar dados da empresa'),
    ('c1d2e3f4-0008-4000-8000-000000000008', 'b1c2d3e4-0003-4000-8000-000000000003', 'EMPRESA_ATUALIZAR', 'Atualizar dados da empresa'),
    -- Recrutador
    ('c1d2e3f4-0009-4000-8000-000000000009', 'b1c2d3e4-0004-4000-8000-000000000004', 'RECRUTADOR_VISUALIZAR', 'Visualizar dados do recrutador'),
    ('c1d2e3f4-0010-4000-8000-000000000010', 'b1c2d3e4-0004-4000-8000-000000000004', 'RECRUTADOR_ATUALIZAR', 'Atualizar dados do recrutador'),
    -- Candidato
    ('c1d2e3f4-0011-4000-8000-000000000011', 'b1c2d3e4-0005-4000-8000-000000000005', 'CANDIDATO_VISUALIZAR', 'Visualizar dados do candidato'),
    ('c1d2e3f4-0012-4000-8000-000000000012', 'b1c2d3e4-0005-4000-8000-000000000005', 'CANDIDATO_ATUALIZAR', 'Atualizar dados do candidato'),
    ('c1d2e3f4-0013-4000-8000-000000000013', 'b1c2d3e4-0005-4000-8000-000000000005', 'CANDIDATO_CRIAR', 'Criar registros de candidato')
ON CONFLICT ("Codigo") DO NOTHING;

-- =============================================
-- SEED DATA: PerfilFuncionalidades
-- =============================================
INSERT INTO "PerfilFuncionalidades" ("Id", "PerfilId", "FuncionalidadeId") VALUES
    -- Admin tem todas
    (uuid_generate_v4(), 'a1b2c3d4-0001-4000-8000-000000000001', 'b1c2d3e4-0001-4000-8000-000000000001'),
    (uuid_generate_v4(), 'a1b2c3d4-0001-4000-8000-000000000001', 'b1c2d3e4-0002-4000-8000-000000000002'),
    (uuid_generate_v4(), 'a1b2c3d4-0001-4000-8000-000000000001', 'b1c2d3e4-0003-4000-8000-000000000003'),
    (uuid_generate_v4(), 'a1b2c3d4-0001-4000-8000-000000000001', 'b1c2d3e4-0004-4000-8000-000000000004'),
    (uuid_generate_v4(), 'a1b2c3d4-0001-4000-8000-000000000001', 'b1c2d3e4-0005-4000-8000-000000000005'),
    -- Gestor
    (uuid_generate_v4(), 'a1b2c3d4-0002-4000-8000-000000000002', 'b1c2d3e4-0002-4000-8000-000000000002'),
    (uuid_generate_v4(), 'a1b2c3d4-0002-4000-8000-000000000002', 'b1c2d3e4-0003-4000-8000-000000000003'),
    -- Recrutador
    (uuid_generate_v4(), 'a1b2c3d4-0003-4000-8000-000000000003', 'b1c2d3e4-0003-4000-8000-000000000003'),
    (uuid_generate_v4(), 'a1b2c3d4-0003-4000-8000-000000000003', 'b1c2d3e4-0004-4000-8000-000000000004'),
    -- Candidato
    (uuid_generate_v4(), 'a1b2c3d4-0004-4000-8000-000000000004', 'b1c2d3e4-0005-4000-8000-000000000005')
ON CONFLICT ("PerfilId", "FuncionalidadeId") DO NOTHING;