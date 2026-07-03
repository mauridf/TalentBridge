-- =============================================
-- TalentBridge - Migration 0004
-- Adiciona colunas faltantes identificadas na
-- auditoria de schema vs entidades
-- =============================================

-- =============================================
-- 1. Adicionar ParceiroId em Empresas
-- Relacionamento: Empresa → Parceiro (N:1)
-- =============================================
ALTER TABLE "Empresas"
    ADD COLUMN "ParceiroId" UUID NULL;

ALTER TABLE "Empresas"
    ADD CONSTRAINT "FK_Empresas_Parceiros"
    FOREIGN KEY ("ParceiroId")
    REFERENCES "Parceiros"("Id")
    ON DELETE SET NULL;

CREATE INDEX IF NOT EXISTS "IX_Empresas_ParceiroId"
    ON "Empresas"("ParceiroId");

-- =============================================
-- 2. Adicionar UpdatedAt em RedefinicoesSenha
-- Herdado de BaseEntity, estava faltando
-- =============================================
ALTER TABLE "RedefinicoesSenha"
    ADD COLUMN "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW();
