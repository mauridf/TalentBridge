# TalentBridge

Plataforma de recrutamento e seleção que conecta **candidatos** a **vagas de emprego**. Permite que empresas publiquem vagas, analisem candidatos via teste comportamental Big Five, gerenciem candidaturas e contratem. Inclui sistema de créditos para publicação de vagas (com pagamento via Asaas), parceiros afiliados e landing pages públicas por empresa.

## Funcionalidades

- Autenticação JWT com multi-perfil e multi-empresa (Candidato, Gestor, Recrutador, Admin)
- Gestão de candidatos com perfil pessoal e profissional (formaçao, experiência, skills)
- Gestão de empresas, gestores e recrutadores via convite
- Publicação e busca de vagas com filtros avançados (regime, tipo, local, salário, competências)
- Candidaturas e processo seletivo (entrevistas, contratação)
- Teste comportamental Big Five (5 traços de personalidade)
- Análise de compatibilidade entre candidato e vaga via IA (OpenAI)
- Sistema de créditos para publicação de vagas com pagamento (Asaas)
- Carrinho de compras, cupons de desconto e histórico de transações
- Programa de parceiros/afiliados com split de pagamentos
- Dashboards com métricas para candidatos, empresas e admin
- Landing pages públicas por empresa com links compartilháveis
- Feed XML de vagas e integração com Google for Jobs
- Armazenamento de arquivos (CVs) via MinIO
- Consulta de CEP (ViaCEP) e geocoding (Nominatim/OpenCage)
- Documentação rica da API com Scalar

## Stack Técnica

| Camada | Tecnologia |
|--------|-----------|
| Backend | .NET 10, C# |
| Banco de Dados | PostgreSQL 16+ |
| ORM | Entity Framework Core 10 |
| Migrations | DbUp (scripts SQL versionados) |
| Autenticação | JWT + BCrypt (work factor 12) |
| Autorização | RBAC por Perfil/Funcionalidade/Operação |
| Validação | FluentValidation |
| Mapeamento | Mapster |
| Documentação | OpenAPI + Scalar |
| Logging | Serilog (console + arquivo) |
| Storage | MinIO |
| Cache | Redis |
| Pagamentos | Asaas (sandbox) |
| IA | OpenAI API |
| Health Checks | ASP.NET Core Health Checks |
| Rate Limiting | ASP.NET Core Rate Limiting |
| Job Scheduling | Hangfire |
| Container | Docker + Docker Compose |
| CI/CD | Render |

## Arquitetura (Clean Architecture)

```
TalentBridge/
├── src/
│   ├── TalentBridge.Domain/        # Entidades, Enums, Value Objects, Interfaces de Repositório
│   ├── TalentBridge.Application/   # DTOs, Services, Validators, Mappings
│   ├── TalentBridge.Infrastructure/ # EF Core (DbContext), Repositories, Serviços Externos
│   └── TalentBridge.Api/           # Controllers, Middlewares, Configurações
├── db/migrations/                  # Scripts SQL versionados (DbUp)
└── tests/                          # Testes unitários e integração
```

### Projetos

- **Domain**: Entidades base (BaseEntity com Id/CreatedAt/UpdatedAt), Usuario TPH (Candidato, Gestor, Recrutador), Value Objects (Endereco), Enums, interfaces de repositório
- **Application**: Casos de uso (Services), DTOs de entrada/saída, validadores FluentValidation, interfaces de serviço
- **Infrastructure**: EF Core DbContext + EntityTypeConfigurations, implementações de repositórios, serviços externos (Email, MinIO, Asaas, OpenAI, ViaCEP, IBGE, Geocoding)
- **Api**: Controllers REST, middlewares (GlobalExceptionHandler), configurações (JWT, CORS, Rate Limiting, Health Checks)

## Modelo de Dados

### Entidades Principais

- **Usuário** (TPH): Candidato, Gestor, Recrutador
- **Empresa**: Segmento, endereço, configurações de notificação
- **Vaga**: Requisitos, localização, competências, Big Five, status
- **Candidatura**: Entrevista, contratação, protocolo
- **PerfilPessoal**: Dados demográficos, ações afirmativas, endereço
- **PerfilProfissional**: Formação acadêmica, experiência profissional, competências, cursos
- **Competência**: Skill compartilhada entre vagas, candidatos e treinamentos
- **Parceiro**: Afiliado com split de pagamentos e código público
- **Treinamento**: Curso com módulos, conteúdos e itens incluídos
- **Créditos**: Produto, Carrinho, Pedido, Histórico de transações
- **Autorização**: Perfil, Funcionalidade, Operação

### Migrations

As migrations são scripts SQL puros executados pelo DbUp em ordem numérica:

- `0001_*_CriarTabelasBase.sql` - Tabelas de autorização, usuários, empresas, segmentos, domínios
- `0002_*_CriarTabelasNegocio.sql` - Tabelas de negócio (candidatos, vagas, candidaturas, etc.)

## Endpoints da API

### Autenticação (`/Usuario`)

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /Usuario/Autenticar | Login (email + senha) |
| POST | /Usuario/SelecionarPerfil | Seleciona perfil (multi-perfil) |
| GET | /Usuario/Empresas | Lista empresas do usuário |
| POST | /Usuario/SelecionarEmpresa | Seleciona empresa (multi-empresa) |
| GET | /Usuario/RefreshAcesso | Refresh token |
| GET | /Usuario/EmailDisponivel | Verifica email |
| PUT | /Usuario/RecuperacaoSenha | Reset de senha |
| POST | /Usuario/ResetSenhaEmail | Envia email de recuperação |

### Candidato (`/Candidato`)

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /Candidato | Criar candidato |
| PUT | /Candidato | Editar candidato |
| GET | /Candidato | Buscar candidato |
| POST | /Candidato/bigfive | Salvar teste Big Five |
| POST | /Candidato/confirmarEmail | Confirmar email |
| POST | /Candidato/perfil-pessoal/upsert | Criar/editar perfil pessoal |
| POST | /Candidato/perfil-profissional/upsert | Criar/editar perfil profissional |

### Empresa (`/Empresa`)

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /Empresa | Criar empresa |
| POST | /Empresa/Editar | Editar empresa |
| GET | /Empresa/{cnpj} | Buscar por CNPJ |
| GET | /Empresa/segmentos | Listar segmentos |
| POST | /Empresa/Dashboard | Dashboard da empresa |
| GET | /Empresa/Dashboard/BancoCandidatos | Banco de candidatos |

### Vaga (`/Vaga`)

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /Vaga/upsert | Criar/editar vaga |
| POST | /Vaga | Listar vagas ativas |
| GET | /Vaga/{id} | Buscar vaga por ID |
| POST | /Vaga/empresa | Vagas de uma empresa |
| POST | /Vaga/encerrar | Encerrar vaga |
| POST | /Vaga/relatorio | Relatório comportamental |

### Admin (`/Admin`)

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /Admin/Dashboard | Dashboard admin |
| POST | /Admin/GeocodeMissing | Varredura de geocode |
| GET | /Admin/Monitor | Diagnóstico do sistema |

## Como Rodar Localmente

### Pré-requisitos

- .NET 10 SDK
- PostgreSQL 16+
- Visual Studio 2022+ ou VS Code
- Docker (opcional, para MinIO e Redis)

### Configuração

```bash
# 1. Configurar connection string (User Secrets)
cd src/TalentBridge.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=talentbridge;Username=postgres;Password=sua_senha"
dotnet user-secrets set "JwtSettings:Secret" "sua-chave-secreta-com-pelo-menos-32-caracteres"

# 2. Criar banco de dados
psql -h localhost -U postgres -c "CREATE DATABASE talentbridge;"

# 3. Executar (migrations rodam automaticamente via DbUp)
dotnet run
```

### Acessar

- **API**: `https://localhost:5001`
- **Scalar (Documentação)**: `https://localhost:5001/scalar/v1`
- **Health Check**: `https://localhost:5001/health`

## Variáveis de Ambiente

| Variável | Descrição | Obrigatório |
|----------|-----------|-------------|
| `ConnectionStrings__DefaultConnection` | Connection string PostgreSQL | Sim |
| `JwtSettings__Secret` | Chave secreta JWT (32+ chars) | Sim |
| `JwtSettings__Issuer` | Emissor do token | Não |
| `JwtSettings__Audience` | Audiência do token | Não |
| `Redis__ConnectionString` | Connection string Redis | Não |
| `MinIO__Endpoint` | Endpoint MinIO | Não |
| `MinIO__AccessKey` | Access key MinIO | Não |
| `MinIO__SecretKey` | Secret key MinIO | Não |
| `MinIO__BucketName` | Bucket padrão | Não |
| `Email__Smtp__Host` | Host SMTP | Não |
| `Email__Smtp__Port` | Porta SMTP | Não |
| `Email__Smtp__Username` | Usuário SMTP | Não |
| `Email__Smtp__Password` | Senha SMTP | Não |
| `Asaas__ApiKey` | Chave da API Asaas | Não |
| `Asaas__BaseUrl` | URL base Asaas (sandbox/prod) | Não |
| `OpenAI__ApiKey` | Chave da API OpenAI | Não |
| `OpenAI__Model` | Modelo OpenAI (default: gpt-4o-mini) | Não |

## Deploy no Render

O deploy é automático via `render.yaml`. Configure as variáveis de ambiente no painel do Render.
