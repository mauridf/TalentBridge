-- =============================================
-- TalentBridge - Migration 0006
-- Ativar candidatos pendentes (pré auto-activation)
-- =============================================

UPDATE "Usuarios"
SET "Status" = 1, -- Ativo
    "UpdatedAt" = NOW()
WHERE "Discriminator" = 'Candidato'
  AND "Status" = 2; -- Pendente
