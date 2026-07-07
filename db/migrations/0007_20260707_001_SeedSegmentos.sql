-- =============================================
-- TalentBridge - Migration 0007
-- Seed de Segmentos de Mercado
-- =============================================

INSERT INTO "Segmentos" ("Id", "Nome", "Descricao") VALUES
    ('a1b2c3d4-0100-4000-8000-000000000001', 'Tecnologia da Informação', 'Empresas de software, hardware, TI e desenvolvimento'),
    ('a1b2c3d4-0100-4000-8000-000000000002', 'Saúde e Bem-Estar', 'Hospitais, clínicas, planos de saúde e serviços médicos'),
    ('a1b2c3d4-0100-4000-8000-000000000003', 'Educação', 'Instituições de ensino, cursos e plataformas educacionais'),
    ('a1b2c3d4-0100-4000-8000-000000000004', 'Serviços Financeiros', 'Bancos, fintechs, corretoras e instituições financeiras'),
    ('a1b2c3d4-0100-4000-8000-000000000005', 'Varejo', 'Lojas físicas, e-commerce e comércio varejista'),
    ('a1b2c3d4-0100-4000-8000-000000000006', 'Indústria e Manufatura', 'Indústrias de transformação, produção e manufatura'),
    ('a1b2c3d4-0100-4000-8000-000000000007', 'Construção Civil', 'Construtoras, incorporadoras e serviços de construção'),
    ('a1b2c3d4-0100-4000-8000-000000000008', 'Agronegócio', 'Agricultura, pecuária e serviços rurais'),
    ('a1b2c3d4-0100-4000-8000-000000000009', 'Telecomunicações', 'Operadoras de telefonia, internet e comunicação'),
    ('a1b2c3d4-0100-4000-8000-000000000010', 'Transporte e Logística', 'Transporte de cargas, passageiros e logística'),
    ('a1b2c3d4-0100-4000-8000-000000000011', 'Consultoria', 'Consultorias empresariais, estratégia e gestão'),
    ('a1b2c3d4-0100-4000-8000-000000000012', 'Marketing e Publicidade', 'Agências de marketing, publicidade e comunicação'),
    ('a1b2c3d4-0100-4000-8000-000000000013', 'Recursos Humanos', 'Empresas de RH, headhunting e gestão de pessoas'),
    ('a1b2c3d4-0100-4000-8000-000000000014', 'Jurídico', 'Escritórios de advocacia e serviços jurídicos'),
    ('a1b2c3d4-0100-4000-8000-000000000015', 'Alimentação', 'Restaurantes, food service e indústria alimentícia'),
    ('a1b2c3d4-0100-4000-8000-000000000016', 'Energia', 'Geração, distribuição e comercialização de energia'),
    ('a1b2c3d4-0100-4000-8000-000000000017', 'Imobiliário', 'Imobiliárias, administração de imóveis e incorporação'),
    ('a1b2c3d4-0100-4000-8000-000000000018', 'Entretenimento e Mídia', 'Produção de conteúdo, streaming, jogos e mídia'),
    ('a1b2c3d4-0100-4000-8000-000000000019', 'Turismo e Hotelaria', 'Hotéis, agências de viagem e serviços turísticos'),
    ('a1b2c3d4-0100-4000-8000-000000000020', 'Moda e Beleza', 'Indústria da moda, cosméticos e cuidados pessoais')
ON CONFLICT ("Id") DO NOTHING;
