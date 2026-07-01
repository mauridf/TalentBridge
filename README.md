# 🚀 TalentBridge

Plataforma de recrutamento e seleção que conecta **candidatos** a **vagas de emprego**.

## 📋 Funcionalidades

- 🔐 Autenticação JWT com multi-perfil e multi-empresa
- 👤 Gestão de candidatos com perfil pessoal e profissional
- 🏢 Gestão de empresas, gestores e recrutadores
- 📋 Publicação e busca de vagas com filtros avançados
- 📝 Candidaturas e processo seletivo (entrevistas, contratação)
- 🧠 Teste comportamental Big Five
- 💰 Sistema de créditos e pagamentos (Asaas)
- 📊 Dashboards com métricas para candidatos, empresas e admin
- 🌐 Landing pages públicas por empresa
- 📚 Documentação da API com Scalar

## 🛠️ Stack Técnica

| Camada | Tecnologia |
|--------|-----------|
| Backend | .NET 9, C# 13 |
| Banco de Dados | PostgreSQL 16+ |
| ORM | Entity Framework Core 9 |
| Autenticação | JWT + BCrypt |
| Validação | FluentValidation |
| Mapeamento | Mapster |
| Migrations | DbUp |
| Documentação | Scalar |
| Logging | Serilog |
| Storage | MinIO |

## 🏗️ Arquitetura
TalentBridge/
├── src/
│ ├── TalentBridge.Domain/ # Entidades, Enums, Value Objects
│ ├── TalentBridge.Application/ # DTOs, Services, Validators
│ ├── TalentBridge.Infrastructure/ # EF Core, Repositories, External Services
│ └── TalentBridge.Api/ # Controllers, Middlewares
├── db/migrations/ # Scripts SQL (DbUp)
└── tests/ # Testes unitários e integração

text

## 🚀 Como Rodar Localmente

### Pré-requisitos
- .NET 9 SDK
- PostgreSQL 16+
- Visual Studio 2022+ ou VS Code

### Configuração
```bash
# 1. Clonar repositório
git clone https://github.com/seu-usuario/talentbridge.git
cd talentbridge

# 2. Configurar connection string (User Secrets)
cd src/TalentBridge.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=talentbridge;Username=postgres;Password=sua_senha;Include Error Detail=true"
dotnet user-secrets set "JwtSettings:Secret" "sua-chave-secreta-com-pelo-menos-32-caracteres"

# 3. Criar banco de dados
psql -h localhost -U postgres -c "CREATE DATABASE talentbridge;"

# 4. Executar
dotnet run
Acessar
API: https://localhost:5001

Scalar (Documentação): https://localhost:5001/scalar/v1

Health Check: https://localhost:5001/health

📚 Documentação da API
Acesse /scalar/v1 para ver a documentação interativa completa.

Principais Endpoints
Método	Rota	Descrição
POST	/Usuario/Autenticar	Login
POST	/Candidato	Criar candidato
POST	/Empresa	Criar empresa
POST	/Vaga/upsert	Criar/editar vaga
POST	/Candidatura	Candidatar-se
POST	/Dashboard/candidato	Dashboard candidato
🌐 Deploy no Render
O deploy é automático via render.yaml. Configure as variáveis de ambiente:

ConnectionStrings__DefaultConnection - Connection string do PostgreSQL

JwtSettings__Secret - Chave secreta JWT

📝 Licença
MIT