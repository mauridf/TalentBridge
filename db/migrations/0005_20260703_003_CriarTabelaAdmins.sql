-- =============================================
-- TalentBridge - Migration 0005
-- Cria a tabela Admins para suporte TPT
-- =============================================

-- =============================================
-- Criar tabela Admins (TPT - herda de Usuarios)
-- =============================================
CREATE TABLE IF NOT EXISTS "Admins" (
    "Id" UUID PRIMARY KEY,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Admins_Usuarios" FOREIGN KEY ("Id")
        REFERENCES "Usuarios"("Id") ON DELETE CASCADE
);

-- =============================================
-- Migrar usuários ADMIN existentes para a
-- tabela Admins (usuários com perfil ADMIN)
-- =============================================
INSERT INTO "Admins" ("Id", "CreatedAt", "UpdatedAt")
SELECT u."Id", u."CreatedAt", u."UpdatedAt"
FROM "Usuarios" u
INNER JOIN "Perfils" p ON u."PerfilId" = p."Id"
WHERE p."Codigo" = 'ADMIN'
  AND NOT EXISTS (SELECT 1 FROM "Admins" a WHERE a."Id" = u."Id");
