-- =============================================
-- Migration 0011: Tornar ConviteId nullable em Recrutadores
-- =============================================
-- Motivo: Recrutador pode ser cadastrado diretamente
-- pelo gestor da empresa (sem convite), mas a coluna
-- era NOT NULL UNIQUE, o que impedia o cadastro direto.
-- =============================================

ALTER TABLE "Recrutadores"
    ALTER COLUMN "ConviteId" DROP NOT NULL,
    DROP CONSTRAINT "FK_Recrutadores_Convites";

ALTER TABLE "Recrutadores"
    ADD CONSTRAINT "FK_Recrutadores_Convites"
        FOREIGN KEY ("ConviteId")
        REFERENCES "Convites"("Id");

DROP INDEX IF EXISTS "IX_Recrutadores_ConviteId";
