-- =============================================
-- TalentBridge - Migration 0003
-- Seed: Usuário Administrador padrão
--
-- ATENÇÃO: Antes de executar, gere o hash BCrypt
-- da senha desejada e substitua abaixo.
--
-- Para gerar o hash, use o próprio projeto:
--   1. Abra um terminal na pasta src/TalentBridge.Api
--   2. Execute: dotnet run -- --generate-hash "sua-senha-aqui"
--   3. Copie o hash gerado e cole na variável abaixo
--
-- Ou use um gerador online (cost factor 11):
--   https://bcrypt-generator.com
-- =============================================

DO $$
DECLARE
    v_admin_perfil_id UUID;
    v_admin_user_id UUID;
    -- 🔽 Hash BCrypt da senha padrão "Admin@123" (cost factor 11)
    v_senha_hash TEXT := '$2a$11$HUKuFbJ5FTtwVQ0te7h3nOVaP.6JAALAekY18Hvf3XGLrNxWAxj2a';
    -- 🔼 Senha padrão: Admin@123 (altere após o primeiro login)
BEGIN

    -- Busca o ID do perfil ADMIN (criado na migration 0001)
    SELECT "Id" INTO v_admin_perfil_id
    FROM "Perfils"
    WHERE "Codigo" = 'ADMIN';

    IF v_admin_perfil_id IS NULL THEN
        RAISE EXCEPTION 'Perfil ADMIN não encontrado. Execute a migration 0001 primeiro.';
    END IF;

    -- Verifica se o usuário admin já existe pelo email
    SELECT "Id" INTO v_admin_user_id
    FROM "Usuarios"
    WHERE "Email" = 'mauridf@gmail.com';

    IF v_admin_user_id IS NULL THEN
        -- Cria o usuário administrador
        INSERT INTO "Usuarios" (
            "Id",
            "Nome",
            "Email",
            "SenhaHash",
            "Telefone",
            "Status",
            "OrigemCadastro",
            "PerfilId",
            "Discriminator",
            "CreatedAt",
            "UpdatedAt"
        ) VALUES (
            gen_random_uuid(),
            'Administrador TalentBridge',
            'mauridf@gmail.com',
            v_senha_hash,
            '',
            1,          -- Ativo
            1,          -- Origem: Web
            v_admin_perfil_id,
            'Usuario',
            NOW(),
            NOW()
        );

        RAISE NOTICE '✅ Usuário administrador criado: mauridf@gmail.com';
    ELSE
        -- Atualiza o perfil para ADMIN (caso o usuário já exista com outro perfil)
        UPDATE "Usuarios"
        SET "PerfilId" = v_admin_perfil_id,
            "Status" = 1,
            "UpdatedAt" = NOW()
        WHERE "Id" = v_admin_user_id;

        RAISE NOTICE '✅ Usuário atualizado para ADMIN: mauridf@gmail.com';
    END IF;

END $$;
