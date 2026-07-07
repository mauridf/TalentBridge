-- =============================================
-- TalentBridge - Migration 0008
-- Seed de Dominios (Lookup Table)
-- =============================================
-- Tipos: 0=RegimeTrabalho, 1=VagaAfirmativa, 2=TipoContratacao,
--        3=JornadaTrabalho, 4=FormacaoAcademica, 5=AreaAtuacao,
--        6=TempoExperiencia, 7=IdentidadeGenero, 8=OrientacaoSexual,
--        9=Pronome, 10=CorRaca, 11=AreaInteresse, 12=OrigemCadastro
-- =============================================

-- Tipo 0: RegimeTrabalho
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0000-4000-8000-000000000001', 1, 'Presencial', 0, 1),
    ('d1e2f3a4-0000-4000-8000-000000000002', 2, 'Remoto', 0, 1),
    ('d1e2f3a4-0000-4000-8000-000000000003', 3, 'Híbrido', 0, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 1: VagaAfirmativa
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0001-4000-8000-000000000001', 1, 'PCD (Pessoa com Deficiência)', 1, 1),
    ('d1e2f3a4-0001-4000-8000-000000000002', 2, 'Mulheres', 1, 1),
    ('d1e2f3a4-0001-4000-8000-000000000003', 3, 'Pessoas Negras', 1, 1),
    ('d1e2f3a4-0001-4000-8000-000000000004', 4, 'LGBTQIAPN+', 1, 1),
    ('d1e2f3a4-0001-4000-8000-000000000005', 5, 'Maiores de 50 anos', 1, 1),
    ('d1e2f3a4-0001-4000-8000-000000000006', 6, 'Jovem Aprendiz', 1, 1),
    ('d1e2f3a4-0001-4000-8000-000000000007', 7, 'Reabilitados INSS', 1, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 2: TipoContratacao
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0002-4000-8000-000000000001', 1, 'CLT', 2, 1),
    ('d1e2f3a4-0002-4000-8000-000000000002', 2, 'PJ', 2, 1),
    ('d1e2f3a4-0002-4000-8000-000000000003', 3, 'Estágio', 2, 1),
    ('d1e2f3a4-0002-4000-8000-000000000004', 4, 'Trainee', 2, 1),
    ('d1e2f3a4-0002-4000-8000-000000000005', 5, 'Temporário', 2, 1),
    ('d1e2f3a4-0002-4000-8000-000000000006', 6, 'Autônomo', 2, 1),
    ('d1e2f3a4-0002-4000-8000-000000000007', 7, 'Cooperado', 2, 1),
    ('d1e2f3a4-0002-4000-8000-000000000008', 8, 'Jovem Aprendiz', 2, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 3: JornadaTrabalho
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0003-4000-8000-000000000001', 1, 'Integral (40h/semana)', 3, 1),
    ('d1e2f3a4-0003-4000-8000-000000000002', 2, 'Meio Período (20h/semana)', 3, 1),
    ('d1e2f3a4-0003-4000-8000-000000000003', 3, 'Horário Flexível', 3, 1),
    ('d1e2f3a4-0003-4000-8000-000000000004', 4, 'Escala 6x1', 3, 1),
    ('d1e2f3a4-0003-4000-8000-000000000005', 5, 'Escala 5x2', 3, 1),
    ('d1e2f3a4-0003-4000-8000-000000000006', 6, 'Turno Rotativo', 3, 1),
    ('d1e2f3a4-0003-4000-8000-000000000007', 7, 'Noturno', 3, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 4: FormacaoAcademica
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0004-4000-8000-000000000001', 1, 'Ensino Fundamental', 4, 1),
    ('d1e2f3a4-0004-4000-8000-000000000002', 2, 'Ensino Médio', 4, 1),
    ('d1e2f3a4-0004-4000-8000-000000000003', 3, 'Técnico', 4, 1),
    ('d1e2f3a4-0004-4000-8000-000000000004', 4, 'Tecnólogo', 4, 1),
    ('d1e2f3a4-0004-4000-8000-000000000005', 5, 'Superior - Bacharelado', 4, 1),
    ('d1e2f3a4-0004-4000-8000-000000000006', 6, 'Superior - Licenciatura', 4, 1),
    ('d1e2f3a4-0004-4000-8000-000000000007', 7, 'Pós-Graduação - Especialização', 4, 1),
    ('d1e2f3a4-0004-4000-8000-000000000008', 8, 'Pós-Graduação - MBA', 4, 1),
    ('d1e2f3a4-0004-4000-8000-000000000009', 9, 'Mestrado', 4, 1),
    ('d1e2f3a4-0004-4000-8000-00000000000a', 10, 'Doutorado', 4, 1),
    ('d1e2f3a4-0004-4000-8000-00000000000b', 11, 'Pós-Doutorado', 4, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 5: AreaAtuacao
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0005-4000-8000-000000000001', 1, 'Administrativo', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000002', 2, 'Tecnologia da Informação', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000003', 3, 'Recursos Humanos', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000004', 4, 'Financeiro', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000005', 5, 'Jurídico', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000006', 6, 'Marketing', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000007', 7, 'Vendas', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000008', 8, 'Operações', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000009', 9, 'Logística', 5, 1),
    ('d1e2f3a4-0005-4000-8000-00000000000a', 10, 'Comercial', 5, 1),
    ('d1e2f3a4-0005-4000-8000-00000000000b', 11, 'Atendimento', 5, 1),
    ('d1e2f3a4-0005-4000-8000-00000000000c', 12, 'Educação', 5, 1),
    ('d1e2f3a4-0005-4000-8000-00000000000d', 13, 'Saúde', 5, 1),
    ('d1e2f3a4-0005-4000-8000-00000000000e', 14, 'Engenharia', 5, 1),
    ('d1e2f3a4-0005-4000-8000-00000000000f', 15, 'Comunicação', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000010', 16, 'Design', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000011', 17, 'Qualidade', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000012', 18, 'Pesquisa e Desenvolvimento', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000013', 19, 'Suprimentos', 5, 1),
    ('d1e2f3a4-0005-4000-8000-000000000014', 20, 'Produção', 5, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 6: TempoExperiencia
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0006-4000-8000-000000000001', 1, 'Estágio (sem experiência)', 6, 1),
    ('d1e2f3a4-0006-4000-8000-000000000002', 2, 'Júnior (1 a 3 anos)', 6, 1),
    ('d1e2f3a4-0006-4000-8000-000000000003', 3, 'Pleno (3 a 6 anos)', 6, 1),
    ('d1e2f3a4-0006-4000-8000-000000000004', 4, 'Sênior (6 a 10 anos)', 6, 1),
    ('d1e2f3a4-0006-4000-8000-000000000005', 5, 'Especialista (mais de 10 anos)', 6, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 7: IdentidadeGenero
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0007-4000-8000-000000000001', 1, 'Homem Cis', 7, 1),
    ('d1e2f3a4-0007-4000-8000-000000000002', 2, 'Homem Trans', 7, 1),
    ('d1e2f3a4-0007-4000-8000-000000000003', 3, 'Mulher Cis', 7, 1),
    ('d1e2f3a4-0007-4000-8000-000000000004', 4, 'Mulher Trans', 7, 1),
    ('d1e2f3a4-0007-4000-8000-000000000005', 5, 'Não Binário', 7, 1),
    ('d1e2f3a4-0007-4000-8000-000000000006', 6, 'Gênero Fluido', 7, 1),
    ('d1e2f3a4-0007-4000-8000-000000000007', 7, 'Agênero', 7, 1),
    ('d1e2f3a4-0007-4000-8000-000000000008', 8, 'Prefiro não informar', 7, 1),
    ('d1e2f3a4-0007-4000-8000-000000000009', 9, 'Outro', 7, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 8: OrientacaoSexual
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0008-4000-8000-000000000001', 1, 'Heterossexual', 8, 1),
    ('d1e2f3a4-0008-4000-8000-000000000002', 2, 'Homossexual (Gay/Lésbica)', 8, 1),
    ('d1e2f3a4-0008-4000-8000-000000000003', 3, 'Bissexual', 8, 1),
    ('d1e2f3a4-0008-4000-8000-000000000004', 4, 'Pansexual', 8, 1),
    ('d1e2f3a4-0008-4000-8000-000000000005', 5, 'Assexual', 8, 1),
    ('d1e2f3a4-0008-4000-8000-000000000006', 6, 'Demissexual', 8, 1),
    ('d1e2f3a4-0008-4000-8000-000000000007', 7, 'Prefiro não informar', 8, 1),
    ('d1e2f3a4-0008-4000-8000-000000000008', 8, 'Outro', 8, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 9: Pronome
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-0009-4000-8000-000000000001', 1, 'Ele/Dele', 9, 1),
    ('d1e2f3a4-0009-4000-8000-000000000002', 2, 'Ela/Dela', 9, 1),
    ('d1e2f3a4-0009-4000-8000-000000000003', 3, 'Elu/Delu', 9, 1),
    ('d1e2f3a4-0009-4000-8000-000000000004', 4, 'Prefiro não informar', 9, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 10: CorRaca
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-000a-4000-8000-000000000001', 1, 'Branca', 10, 1),
    ('d1e2f3a4-000a-4000-8000-000000000002', 2, 'Preta', 10, 1),
    ('d1e2f3a4-000a-4000-8000-000000000003', 3, 'Parda', 10, 1),
    ('d1e2f3a4-000a-4000-8000-000000000004', 4, 'Amarela', 10, 1),
    ('d1e2f3a4-000a-4000-8000-000000000005', 5, 'Indígena', 10, 1),
    ('d1e2f3a4-000a-4000-8000-000000000006', 6, 'Prefiro não informar', 10, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 11: AreaInteresse
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-000b-4000-8000-000000000001', 1, 'Tecnologia', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000002', 2, 'Saúde', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000003', 3, 'Educação', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000004', 4, 'Administração', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000005', 5, 'Marketing', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000006', 6, 'Finanças', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000007', 7, 'Recursos Humanos', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000008', 8, 'Jurídico', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000009', 9, 'Engenharia', 11, 1),
    ('d1e2f3a4-000b-4000-8000-00000000000a', 10, 'Design', 11, 1),
    ('d1e2f3a4-000b-4000-8000-00000000000b', 11, 'Vendas', 11, 1),
    ('d1e2f3a4-000b-4000-8000-00000000000c', 12, 'Comunicação', 11, 1),
    ('d1e2f3a4-000b-4000-8000-00000000000d', 13, 'Logística', 11, 1),
    ('d1e2f3a4-000b-4000-8000-00000000000e', 14, 'Meio Ambiente', 11, 1),
    ('d1e2f3a4-000b-4000-8000-00000000000f', 15, 'Artes e Cultura', 11, 1),
    ('d1e2f3a4-000b-4000-8000-000000000010', 16, 'Esportes', 11, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;

-- Tipo 12: OrigemCadastro
INSERT INTO "Dominios" ("Id", "Codigo", "Descricao", "Tipo", "Status") VALUES
    ('d1e2f3a4-000c-4000-8000-000000000001', 1, 'Web', 12, 1),
    ('d1e2f3a4-000c-4000-8000-000000000002', 2, 'WhatsApp', 12, 1),
    ('d1e2f3a4-000c-4000-8000-000000000003', 3, 'LinkedIn', 12, 1),
    ('d1e2f3a4-000c-4000-8000-000000000004', 4, 'Indicação', 12, 1),
    ('d1e2f3a4-000c-4000-8000-000000000005', 5, 'Google', 12, 1),
    ('d1e2f3a4-000c-4000-8000-000000000006', 6, 'Facebook', 12, 1),
    ('d1e2f3a4-000c-4000-8000-000000000007', 7, 'Instagram', 12, 1),
    ('d1e2f3a4-000c-4000-8000-000000000008', 8, 'Outro', 12, 1)
ON CONFLICT ("Tipo", "Codigo") DO NOTHING;
