-- =============================================
-- TalentBridge - Migration 0002
-- Tabelas de negócio: Candidatos, Vagas, etc.
-- =============================================

-- =============================================
-- 1. Tabela: Candidatos (TPH - herda de Usuarios)
-- =============================================
CREATE TABLE IF NOT EXISTS "Candidatos" (
    "Id" UUID PRIMARY KEY,
    "NomeSocial" VARCHAR(100) NULL,
    "DataNascimento" TIMESTAMP WITH TIME ZONE NOT NULL,
    "TokenConfirmacaoEmail" UUID NULL,
    "Extroversao" INTEGER NULL,
    "Amabilidade" INTEGER NULL,
    "Consciencia" INTEGER NULL,
    "EstabilidadeEmocional" INTEGER NULL,
    "AberturaExperiencia" INTEGER NULL,
    "DataUltimoTesteBigFive" TIMESTAMP WITH TIME ZONE NULL,
    "PerfilPessoalId" UUID NULL,
    "PerfilProfissionalId" UUID NULL,
    "ParceiroId" UUID NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Candidatos_Usuarios" FOREIGN KEY ("Id") REFERENCES "Usuarios"("Id") ON DELETE CASCADE
);

-- =============================================
-- 2. Tabela: Gestores (TPH - herda de Usuarios)
-- =============================================
CREATE TABLE IF NOT EXISTS "Gestores" (
    "Id" UUID PRIMARY KEY,
    "EmpresaId" UUID NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Gestores_Usuarios" FOREIGN KEY ("Id") REFERENCES "Usuarios"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Gestores_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id")
);

CREATE INDEX IF NOT EXISTS "IX_Gestores_EmpresaId" ON "Gestores"("EmpresaId");

-- =============================================
-- 3. Tabela: Convites
-- =============================================
CREATE TABLE IF NOT EXISTS "Convites" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Email" VARCHAR(200) NOT NULL,
    "Cnpj" VARCHAR(14) NULL,
    "NomeEmpresa" VARCHAR(128) NULL,
    "NomeResponsavel" VARCHAR(128) NULL,
    "Telefone" VARCHAR(20) NULL,
    "Status" INTEGER NOT NULL DEFAULT 0,
    "Tipo" INTEGER NOT NULL,
    "Token" UUID NOT NULL DEFAULT uuid_generate_v4(),
    "DataExpiracao" TIMESTAMP WITH TIME ZONE NOT NULL,
    "DataAceite" TIMESTAMP WITH TIME ZONE NULL,
    "EmpresaResponsavelId" UUID NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Convites_EmpresaResponsavel" FOREIGN KEY ("EmpresaResponsavelId") REFERENCES "Empresas"("Id")
);

-- =============================================
-- 4. Tabela: Recrutadores (TPH - herda de Usuarios)
-- =============================================
CREATE TABLE IF NOT EXISTS "Recrutadores" (
    "Id" UUID PRIMARY KEY,
    "NomeSocial" VARCHAR(200) NULL,
    "EmpresaId" UUID NOT NULL,
    "ConviteId" UUID NOT NULL UNIQUE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Recrutadores_Usuarios" FOREIGN KEY ("Id") REFERENCES "Usuarios"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Recrutadores_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id"),
    CONSTRAINT "FK_Recrutadores_Convites" FOREIGN KEY ("ConviteId") REFERENCES "Convites"("Id")
);

CREATE INDEX IF NOT EXISTS "IX_Recrutadores_EmpresaId" ON "Recrutadores"("EmpresaId");

-- =============================================
-- 5. Tabela: Vagas
-- =============================================
CREATE TABLE IF NOT EXISTS "Vagas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "EmpresaId" UUID NOT NULL,
    "UsuarioCriacaoId" UUID NOT NULL,
    "UsuarioEncerramentoId" UUID NULL,
    "Titulo" VARCHAR(100) NOT NULL,
    "Cargo" VARCHAR(100) NOT NULL,
    "Descricao" VARCHAR(2000) NOT NULL,
    "Atividades" VARCHAR(1000) NOT NULL,
    "Beneficios" VARCHAR(1000) NOT NULL,
    "DiferenciaisConsiderados" VARCHAR(1000) NOT NULL,
    "Salario" DECIMAL(18,2) NOT NULL,
    "RegimeTrabalho" INTEGER NOT NULL,
    "JornadaTrabalho" INTEGER NOT NULL,
    "TipoContratacao" INTEGER NOT NULL,
    "FormacaoAcademica" INTEGER NOT NULL,
    "FormacaoAcademicaEliminatorio" BOOLEAN NOT NULL DEFAULT FALSE,
    "AreaAtuacao" INTEGER NOT NULL,
    "TempoExperiencia" INTEGER NOT NULL,
    "TempoExperienciaEliminatorio" BOOLEAN NOT NULL DEFAULT FALSE,
    "AcoesAfirmativas" VARCHAR(500) NULL,
    "IdadeMinima" INTEGER NULL,
    "IdadeMaxima" INTEGER NULL,
    "OcupacaoAnteriorCargo" BOOLEAN NOT NULL DEFAULT FALSE,
    "DisponibilidadeDeslocamento" BOOLEAN NOT NULL DEFAULT FALSE,
    "Estado" VARCHAR(100) NOT NULL,
    "Cidade" VARCHAR(100) NOT NULL,
    "CEP" VARCHAR(8) NULL,
    "Rua" VARCHAR(100) NULL,
    "Numero" VARCHAR(10) NULL,
    "Bairro" VARCHAR(100) NULL,
    "Complemento" VARCHAR(200) NULL,
    "UtilizaEnderecoCadastrado" BOOLEAN NOT NULL DEFAULT FALSE,
    "Latitude" DOUBLE PRECISION NULL,
    "Longitude" DOUBLE PRECISION NULL,
    "Status" INTEGER NOT NULL DEFAULT 1,
    "Encerrada" BOOLEAN NOT NULL DEFAULT FALSE,
    "DataCandidaturaInicio" TIMESTAMP WITH TIME ZONE NOT NULL,
    "DataCandidaturaFim" TIMESTAMP WITH TIME ZONE NOT NULL,
    "DataEncerramento" TIMESTAMP WITH TIME ZONE NULL,
    "TipoVaga" INTEGER NOT NULL DEFAULT 1,
    "LinkExterno" VARCHAR(500) NULL,
    "QuantidadeVagas" INTEGER NOT NULL DEFAULT 1,
    "RecrutamentoWhatsApp" BOOLEAN NOT NULL DEFAULT FALSE,
    "DataRecrutamentoWhatsApp" TIMESTAMP WITH TIME ZONE NULL,
    "DataPublicacaoGoogle" TIMESTAMP WITH TIME ZONE NULL,
    "ExtroversaoMinima" INTEGER NULL,
    "AmabilidadeMinima" INTEGER NULL,
    "AutodisciplinaMinima" INTEGER NULL,
    "EstabilidadeEmocionalMinima" INTEGER NULL,
    "AberturaExperienciaMinima" INTEGER NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Vagas_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id"),
    CONSTRAINT "FK_Vagas_UsuarioCriacao" FOREIGN KEY ("UsuarioCriacaoId") REFERENCES "Usuarios"("Id"),
    CONSTRAINT "FK_Vagas_UsuarioEncerramento" FOREIGN KEY ("UsuarioEncerramentoId") REFERENCES "Usuarios"("Id")
);

CREATE INDEX IF NOT EXISTS "IX_Vagas_EmpresaId" ON "Vagas"("EmpresaId");
CREATE INDEX IF NOT EXISTS "IX_Vagas_Status" ON "Vagas"("Status");
CREATE INDEX IF NOT EXISTS "IX_Vagas_Cidade" ON "Vagas"("Cidade");
CREATE INDEX IF NOT EXISTS "IX_Vagas_Estado" ON "Vagas"("Estado");
CREATE INDEX IF NOT EXISTS "IX_Vagas_DataCandidaturaFim" ON "Vagas"("DataCandidaturaFim");

-- =============================================
-- 6. Tabela: Candidaturas
-- =============================================
CREATE TABLE IF NOT EXISTS "Candidaturas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "VagaId" UUID NOT NULL,
    "CandidatoId" UUID NOT NULL,
    "Contratado" BOOLEAN NOT NULL DEFAULT FALSE,
    "EntrevistaRealizada" BOOLEAN NOT NULL DEFAULT FALSE,
    "DataHoraEntrevista" TIMESTAMP WITH TIME ZONE NULL,
    "LinkEntrevista" VARCHAR(500) NULL,
    "MeioEntrevista" VARCHAR(50) NULL,
    "DuracaoEntrevistaMinutos" INTEGER NULL,
    "Origem" INTEGER NULL,
    "Protocolo" VARCHAR(20) NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Candidaturas_Vagas" FOREIGN KEY ("VagaId") REFERENCES "Vagas"("Id"),
    CONSTRAINT "FK_Candidaturas_Candidatos" FOREIGN KEY ("CandidatoId") REFERENCES "Candidatos"("Id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Candidaturas_VagaCandidato" ON "Candidaturas"("VagaId", "CandidatoId");
CREATE INDEX IF NOT EXISTS "IX_Candidaturas_VagaId" ON "Candidaturas"("VagaId");
CREATE INDEX IF NOT EXISTS "IX_Candidaturas_CandidatoId" ON "Candidaturas"("CandidatoId");

-- =============================================
-- 6. Tabela: Visitas
-- =============================================
CREATE TABLE IF NOT EXISTS "Visitas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "VagaId" UUID NOT NULL,
    "CandidatoId" UUID NOT NULL,
    "DataVisita" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "Ip" VARCHAR(50) NULL,
    "UserAgent" VARCHAR(500) NULL,
    "Origem" VARCHAR(100) NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Visitas_Vagas" FOREIGN KEY ("VagaId") REFERENCES "Vagas"("Id"),
    CONSTRAINT "FK_Visitas_Candidatos" FOREIGN KEY ("CandidatoId") REFERENCES "Candidatos"("Id")
);

-- =============================================
-- 7. Tabela: PerfisPessoais
-- =============================================
CREATE TABLE IF NOT EXISTS "PerfisPessoais" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "CorRaca" INTEGER NULL,
    "Pronome" INTEGER NULL,
    "IdentidadeGenero" INTEGER NULL,
    "OrientacaoSexual" INTEGER NULL,
    "Cpf" VARCHAR(14) NULL,
    "Rg" VARCHAR(20) NULL,
    "SobreMim" TEXT NULL,
    "LocalResidencia" VARCHAR(200) NULL,
    "AcoesAfirmativas" TEXT NULL,
    "DescricaoPcd" TEXT NULL,
    "Instagram" VARCHAR(200) NULL,
    "Facebook" VARCHAR(200) NULL,
    "Linkedin" VARCHAR(200) NULL,
    "CEP" VARCHAR(8) NULL,
    "Rua" VARCHAR(200) NULL,
    "Numero" VARCHAR(10) NULL,
    "Bairro" VARCHAR(100) NULL,
    "Cidade" VARCHAR(100) NULL,
    "Estado" VARCHAR(2) NULL,
    "Complemento" VARCHAR(200) NULL,
    "Latitude" DOUBLE PRECISION NULL,
    "Longitude" DOUBLE PRECISION NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_PerfisPessoais_Cpf" ON "PerfisPessoais"("Cpf") WHERE "Cpf" IS NOT NULL;

-- =============================================
-- 8. Tabela: PerfisProfissionais
-- =============================================
CREATE TABLE IF NOT EXISTS "PerfisProfissionais" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "DispensaExperienciaProfissional" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- 9. Tabela: FormacoesAcademicas
-- =============================================
CREATE TABLE IF NOT EXISTS "FormacoesAcademicas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PerfilProfissionalId" UUID NOT NULL,
    "Grau" VARCHAR(100) NOT NULL,
    "AreaAtuacao" VARCHAR(100) NOT NULL,
    "DataConclusao" TIMESTAMP WITH TIME ZONE NULL,
    "Concluido" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_FormacoesAcademicas_PerfilProfissional" FOREIGN KEY ("PerfilProfissionalId")
        REFERENCES "PerfisProfissionais"("Id") ON DELETE CASCADE
);

-- =============================================
-- 10. Tabela: ExperienciasProfissionais
-- =============================================
CREATE TABLE IF NOT EXISTS "ExperienciasProfissionais" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PerfilProfissionalId" UUID NOT NULL,
    "Empresa" VARCHAR(200) NOT NULL,
    "Posicao" VARCHAR(200) NOT NULL,
    "DataInicio" TIMESTAMP WITH TIME ZONE NOT NULL,
    "DataFim" TIMESTAMP WITH TIME ZONE NULL,
    "EmpregoAtual" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_ExperienciasProfissionais_PerfilProfissional" FOREIGN KEY ("PerfilProfissionalId")
        REFERENCES "PerfisProfissionais"("Id") ON DELETE CASCADE
);

-- =============================================
-- 11. Tabela: Competencias
-- =============================================
CREATE TABLE IF NOT EXISTS "Competencias" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Nome" VARCHAR(100) NOT NULL,
    "EmpresaId" UUID NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Competencias_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id") ON DELETE SET NULL
);

-- =============================================
-- 12. Tabela: CompetenciasVagas
-- =============================================
CREATE TABLE IF NOT EXISTS "CompetenciasVagas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "VagaId" UUID NOT NULL,
    "CompetenciaId" UUID NOT NULL,
    "Nivel" INTEGER NOT NULL,
    "Peso" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_CompetenciasVagas_Vagas" FOREIGN KEY ("VagaId") REFERENCES "Vagas"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CompetenciasVagas_Competencias" FOREIGN KEY ("CompetenciaId") REFERENCES "Competencias"("Id") ON DELETE CASCADE
);

-- =============================================
-- 13. Tabela: CompetenciasCandidatos
-- =============================================
CREATE TABLE IF NOT EXISTS "CompetenciasCandidatos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PerfilProfissionalId" UUID NOT NULL,
    "CompetenciaId" UUID NOT NULL,
    "Nivel" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_CompetenciasCandidatos_PerfilProfissional" FOREIGN KEY ("PerfilProfissionalId")
        REFERENCES "PerfisProfissionais"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CompetenciasCandidatos_Competencias" FOREIGN KEY ("CompetenciaId")
        REFERENCES "Competencias"("Id") ON DELETE CASCADE
);

-- =============================================
-- 14. Tabela: CompetenciasTreinamentos
-- =============================================
CREATE TABLE IF NOT EXISTS "CompetenciasTreinamentos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "TreinamentoId" UUID NOT NULL,
    "CompetenciaId" UUID NOT NULL,
    "Nivel" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_CompetenciasTreinamentos_Treinamentos" FOREIGN KEY ("TreinamentoId")
        REFERENCES "Treinamentos"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CompetenciasTreinamentos_Competencias" FOREIGN KEY ("CompetenciaId")
        REFERENCES "Competencias"("Id") ON DELETE CASCADE
);

-- =============================================
-- 15. Tabela: Cursos
-- =============================================
CREATE TABLE IF NOT EXISTS "Cursos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PerfilProfissionalId" UUID NOT NULL,
    "VagaId" UUID NULL,
    "NomeCurso" VARCHAR(50) NOT NULL,
    "DataConclusao" TIMESTAMP WITH TIME ZONE NULL,
    "Concluido" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Cursos_PerfilProfissional" FOREIGN KEY ("PerfilProfissionalId")
        REFERENCES "PerfisProfissionais"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Cursos_Vagas" FOREIGN KEY ("VagaId") REFERENCES "Vagas"("Id") ON DELETE SET NULL
);

-- =============================================
-- 16. Tabela: AreasInteresse
-- =============================================
CREATE TABLE IF NOT EXISTS "AreasInteresse" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Codigo" INTEGER NOT NULL UNIQUE,
    "Nome" VARCHAR(100) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- 17. Tabela: AreasInteressePerfisProfissionais (N:N)
-- =============================================
CREATE TABLE IF NOT EXISTS "AreasInteressePerfisProfissionais" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "AreaInteresseId" UUID NOT NULL,
    "PerfilProfissionalId" UUID NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_AreasInt_PerfisProf_AreaInteresse" FOREIGN KEY ("AreaInteresseId")
        REFERENCES "AreasInteresse"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AreasInt_PerfisProf_PerfilProfissional" FOREIGN KEY ("PerfilProfissionalId")
        REFERENCES "PerfisProfissionais"("Id") ON DELETE CASCADE,
    CONSTRAINT "UQ_AreaInteressePerfilProfissional" UNIQUE ("AreaInteresseId", "PerfilProfissionalId")
);

-- =============================================
-- 18. Tabela: Parceiros
-- =============================================
CREATE TABLE IF NOT EXISTS "Parceiros" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Nome" VARCHAR(100) NOT NULL,
    "NomeSocial" VARCHAR(200) NULL,
    "Email" VARCHAR(200) NOT NULL UNIQUE,
    "Telefone" VARCHAR(20) NULL,
    "RendaMensal" DECIMAL(18,2) NOT NULL DEFAULT 0,
    "TipoPessoa" VARCHAR(2) NOT NULL DEFAULT 'PF',
    "Documento" VARCHAR(14) NOT NULL UNIQUE,
    "CodigoPublico" VARCHAR(20) NOT NULL UNIQUE,
    "Origem" VARCHAR(200) NULL,
    "Status" INTEGER NOT NULL DEFAULT 1,
    "WalletId" VARCHAR(100) NULL,
    "PercentualSplit" DECIMAL(5,2) NOT NULL DEFAULT 0,
    "CEP" VARCHAR(8) NULL,
    "Rua" VARCHAR(200) NULL,
    "Numero" VARCHAR(10) NULL,
    "Bairro" VARCHAR(100) NULL,
    "Cidade" VARCHAR(100) NULL,
    "Estado" VARCHAR(2) NULL,
    "Complemento" VARCHAR(200) NULL,
    "Latitude" DOUBLE PRECISION NULL,
    "Longitude" DOUBLE PRECISION NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- 19. Tabela: Cupons
-- =============================================
CREATE TABLE IF NOT EXISTS "Cupons" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Nome" VARCHAR(150) NOT NULL,
    "PercentualDesconto" INTEGER NOT NULL,
    "DataValidade" TIMESTAMP WITH TIME ZONE NOT NULL,
    "ParceiroId" UUID NULL,
    "Status" INTEGER NOT NULL DEFAULT 1,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Cupons_Parceiros" FOREIGN KEY ("ParceiroId") REFERENCES "Parceiros"("Id") ON DELETE SET NULL
);

-- =============================================
-- 21. Tabela: UsuariosEmpresas
-- =============================================
CREATE TABLE IF NOT EXISTS "UsuariosEmpresas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UsuarioId" UUID NOT NULL,
    "EmpresaId" UUID NOT NULL,
    "PerfilId" UUID NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_UsuariosEmpresas_Usuarios" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UsuariosEmpresas_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UsuariosEmpresas_Perfis" FOREIGN KEY ("PerfilId") REFERENCES "Perfils"("Id"),
    CONSTRAINT "UQ_UsuarioEmpresa" UNIQUE ("UsuarioId", "EmpresaId")
);

-- =============================================
-- 22. Tabela: Produtos
-- =============================================
CREATE TABLE IF NOT EXISTS "Produtos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "NomeProduto" VARCHAR(150) NOT NULL,
    "DescricaoProduto" VARCHAR(300) NULL,
    "ValorProduto" DECIMAL(18,2) NOT NULL,
    "QuantidadeCandidatos" INTEGER NULL,
    "QuantidadeCreditoPorVaga" INTEGER NOT NULL DEFAULT 1,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- 23. Tabela: CreditosEmpresas
-- =============================================
CREATE TABLE IF NOT EXISTS "CreditosEmpresas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "EmpresaId" UUID NOT NULL,
    "ProdutoId" UUID NOT NULL,
    "Creditos" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_CreditosEmpresas_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CreditosEmpresas_Produtos" FOREIGN KEY ("ProdutoId") REFERENCES "Produtos"("Id") ON DELETE CASCADE,
    CONSTRAINT "UQ_CreditoEmpresaProduto" UNIQUE ("EmpresaId", "ProdutoId")
);

-- =============================================
-- 24. Tabela: CreditoVagas
-- =============================================
CREATE TABLE IF NOT EXISTS "CreditoVagas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "EmpresaId" UUID NOT NULL,
    "VagaId" UUID NOT NULL,
    "ProdutoId" UUID NOT NULL,
    "CreditoEmpresaId" UUID NOT NULL,
    "QuantidadeLiberada" INTEGER NOT NULL DEFAULT 1,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_CreditoVagas_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id"),
    CONSTRAINT "FK_CreditoVagas_Vagas" FOREIGN KEY ("VagaId") REFERENCES "Vagas"("Id"),
    CONSTRAINT "FK_CreditoVagas_Produtos" FOREIGN KEY ("ProdutoId") REFERENCES "Produtos"("Id"),
    CONSTRAINT "FK_CreditoVagas_CreditosEmpresas" FOREIGN KEY ("CreditoEmpresaId") REFERENCES "CreditosEmpresas"("Id")
);

-- =============================================
-- 25. Tabela: Carrinhos
-- =============================================
CREATE TABLE IF NOT EXISTS "Carrinhos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "EmpresaId" UUID NOT NULL,
    "Status" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Carrinhos_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id")
);

-- =============================================
-- 26. Tabela: ItensCarrinho
-- =============================================
CREATE TABLE IF NOT EXISTS "ItensCarrinho" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "CarrinhoId" UUID NOT NULL,
    "ProdutoId" UUID NOT NULL,
    "Quantidade" INTEGER NOT NULL,
    "ValorUnitario" DECIMAL(18,2) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_ItensCarrinho_Carrinhos" FOREIGN KEY ("CarrinhoId") REFERENCES "Carrinhos"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ItensCarrinho_Produtos" FOREIGN KEY ("ProdutoId") REFERENCES "Produtos"("Id")
);

-- =============================================
-- 27. Tabela: Pedidos
-- =============================================
CREATE TABLE IF NOT EXISTS "Pedidos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "NumeroPedido" INTEGER NOT NULL UNIQUE,
    "CarrinhoId" UUID NOT NULL UNIQUE,
    "EmpresaId" UUID NOT NULL,
    "CupomId" UUID NULL,
    "IdCheckout" VARCHAR(100) NULL,
    "Status" INTEGER NOT NULL DEFAULT 0,
    "CreditosInseridos" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Pedidos_Carrinhos" FOREIGN KEY ("CarrinhoId") REFERENCES "Carrinhos"("Id"),
    CONSTRAINT "FK_Pedidos_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id"),
    CONSTRAINT "FK_Pedidos_Cupons" FOREIGN KEY ("CupomId") REFERENCES "Cupons"("Id") ON DELETE SET NULL
);

-- =============================================
-- 28. Tabela: ItensPedido
-- =============================================
CREATE TABLE IF NOT EXISTS "ItensPedido" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PedidoId" UUID NOT NULL,
    "ProdutoId" UUID NOT NULL,
    "Quantidade" INTEGER NOT NULL,
    "ValorUnitario" DECIMAL(18,2) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_ItensPedido_Pedidos" FOREIGN KEY ("PedidoId") REFERENCES "Pedidos"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ItensPedido_Produtos" FOREIGN KEY ("ProdutoId") REFERENCES "Produtos"("Id")
);

-- =============================================
-- 29. Tabela: HistoricoTransacoes
-- =============================================
CREATE TABLE IF NOT EXISTS "HistoricoTransacoes" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "EmpresaId" UUID NOT NULL,
    "CnpjEmpresa" VARCHAR(14) NOT NULL,
    "VagaId" UUID NULL,
    "CreditoEmpresaId" UUID NULL,
    "CreditoVagaId" UUID NULL,
    "DescricaoTransacao" VARCHAR(350) NOT NULL,
    "PerfilResponsavel" VARCHAR(150) NOT NULL,
    "DataTransacao" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "ValorPago" DECIMAL(18,2) NULL,
    "AlteracaoCredito" INTEGER NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_HistoricoTransacoes_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id"),
    CONSTRAINT "FK_HistoricoTransacoes_Vagas" FOREIGN KEY ("VagaId") REFERENCES "Vagas"("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_HistoricoTransacoes_CreditosEmpresas" FOREIGN KEY ("CreditoEmpresaId") REFERENCES "CreditosEmpresas"("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_HistoricoTransacoes_CreditoVagas" FOREIGN KEY ("CreditoVagaId") REFERENCES "CreditoVagas"("Id") ON DELETE SET NULL
);

-- =============================================
-- 30. Tabela: Treinamentos
-- =============================================
CREATE TABLE IF NOT EXISTS "Treinamentos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "NomeCurso" VARCHAR(200) NOT NULL,
    "Categoria" VARCHAR(100) NULL,
    "Nivel" VARCHAR(50) NULL,
    "Descricao" VARCHAR(2000) NULL,
    "ResultadosAprendizagem" VARCHAR(2000) NULL,
    "Criador" VARCHAR(100) NULL,
    "Avaliacao" DOUBLE PRECISION NOT NULL DEFAULT 0,
    "QuantidadeAvaliacoes" INTEGER NOT NULL DEFAULT 0,
    "DuracaoMinutos" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- 31. Tabela: TreinamentoVagas (N:N)
-- =============================================
CREATE TABLE IF NOT EXISTS "TreinamentoVagas" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "TreinamentoId" UUID NOT NULL,
    "VagaId" UUID NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_TreinamentoVagas_Treinamentos" FOREIGN KEY ("TreinamentoId")
        REFERENCES "Treinamentos"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_TreinamentoVagas_Vagas" FOREIGN KEY ("VagaId")
        REFERENCES "Vagas"("Id") ON DELETE CASCADE,
    CONSTRAINT "UQ_TreinamentoVaga" UNIQUE ("TreinamentoId", "VagaId")
);

-- =============================================
-- 32. Tabela: ModulosCursos
-- =============================================
CREATE TABLE IF NOT EXISTS "ModulosCursos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "TreinamentoId" UUID NOT NULL,
    "Nome" VARCHAR(200) NOT NULL,
    "Descricao" VARCHAR(1000) NULL,
    "Ordem" INTEGER NOT NULL DEFAULT 0,
    "DuracaoMinutos" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_ModulosCursos_Treinamentos" FOREIGN KEY ("TreinamentoId")
        REFERENCES "Treinamentos"("Id") ON DELETE CASCADE
);

-- =============================================
-- 33. Tabela: ConteudosModulos
-- =============================================
CREATE TABLE IF NOT EXISTS "ConteudosModulos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "ModuloCursoId" UUID NOT NULL,
    "Titulo" VARCHAR(200) NOT NULL,
    "Descricao" VARCHAR(2000) NULL,
    "UrlVideo" VARCHAR(500) NULL,
    "UrlMaterial" VARCHAR(500) NULL,
    "Ordem" INTEGER NOT NULL DEFAULT 0,
    "DuracaoMinutos" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_ConteudosModulos_ModulosCursos" FOREIGN KEY ("ModuloCursoId")
        REFERENCES "ModulosCursos"("Id") ON DELETE CASCADE
);

-- =============================================
-- 34. Tabela: ItensIncluidos
-- =============================================
CREATE TABLE IF NOT EXISTS "ItensIncluidos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "TreinamentoId" UUID NOT NULL,
    "Descricao" VARCHAR(500) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_ItensIncluidos_Treinamentos" FOREIGN KEY ("TreinamentoId")
        REFERENCES "Treinamentos"("Id") ON DELETE CASCADE
);

-- =============================================
-- 35. Tabela: Contatos
-- =============================================
CREATE TABLE IF NOT EXISTS "Contatos" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UsuarioId" UUID NULL,
    "EmpresaId" UUID NULL,
    "Nome" VARCHAR(200) NOT NULL,
    "Email" VARCHAR(200) NOT NULL,
    "Telefone" VARCHAR(20) NULL,
    "Mensagem" VARCHAR(2000) NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Contatos_Usuarios" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios"("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Contatos_Empresas" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id") ON DELETE SET NULL
);

-- =============================================
-- 36. Tabela: ParametrosGerais
-- =============================================
CREATE TABLE IF NOT EXISTS "ParametrosGerais" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Chave" VARCHAR(100) NOT NULL UNIQUE,
    "Valor" VARCHAR(500) NOT NULL,
    "Descricao" VARCHAR(500) NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- =============================================
-- 37. Tabela: ApiLogs
-- =============================================
CREATE TABLE IF NOT EXISTS "ApiLogs" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Metodo" VARCHAR(10) NOT NULL,
    "Rota" VARCHAR(500) NOT NULL,
    "StatusCode" INTEGER NOT NULL,
    "DuracaoMs" BIGINT NOT NULL DEFAULT 0,
    "Ip" VARCHAR(50) NULL,
    "UserAgent" VARCHAR(500) NULL,
    "UsuarioId" UUID NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS "IX_ApiLogs_CreatedAt" ON "ApiLogs"("CreatedAt");
CREATE INDEX IF NOT EXISTS "IX_ApiLogs_StatusCode" ON "ApiLogs"("StatusCode");
