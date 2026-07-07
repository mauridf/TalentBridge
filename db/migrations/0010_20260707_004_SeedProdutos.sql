-- =============================================
-- TalentBridge - Migration 0010
-- Seed de Produtos (Pacotes de Créditos)
-- =============================================

INSERT INTO "Produtos" ("Id", "NomeProduto", "DescricaoProduto", "ValorProduto", "QuantidadeCreditoPorVaga", "QuantidadeCandidatos") VALUES
    ('f1a2b3c4-0001-4000-8000-000000000001', 'Básico', 'Acesso básico ao banco de currículos', 49.90, 1, 50),
    ('f1a2b3c4-0001-4000-8000-000000000002', 'Profissional', 'Acesso profissional com mais créditos', 99.90, 3, 150),
    ('f1a2b3c4-0001-4000-8000-000000000003', 'Avançado', 'Acesso avançado com créditos ilimitados', 199.90, 10, 500),
    ('f1a2b3c4-0001-4000-8000-000000000004', 'Enterprise', 'Solução completa para grandes empresas', 499.90, 30, 9999)
ON CONFLICT ("Id") DO NOTHING;
