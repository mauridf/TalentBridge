using FluentResults;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Candidato;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de candidatos
/// </summary>
public class CandidatoService : ICandidatoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IValidator<CriarCandidatoRequestDto> _criarValidator;
    private readonly ILogger<CandidatoService> _logger;

    // ID do perfil Candidato (deve vir do banco, mas para simplificar usamos o GUID do seed)
    private static readonly Guid PerfilCandidatoId = Guid.Parse("a1b2c3d4-0004-4000-8000-000000000004");

    public CandidatoService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IValidator<CriarCandidatoRequestDto> criarValidator,
        ILogger<CandidatoService> logger)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _criarValidator = criarValidator;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo candidato
    /// </summary>
    public async Task<Result<CriarCandidatoResponseDto>> CriarAsync(
        CriarCandidatoRequestDto request,
        CancellationToken cancellationToken = default)
    {
        // Validar request
        var validationResult = await _criarValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var erros = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result.Fail<CriarCandidatoResponseDto>(erros);
        }

        // Verificar se email já existe
        var emailExiste = await _unitOfWork.Usuarios.EmailExisteAsync(request.Email, cancellationToken);
        if (emailExiste)
        {
            return Result.Fail<CriarCandidatoResponseDto>("EMAIL_JA_EXISTENTE");
        }

        // Hash da senha
        var senhaHash = _authService.HashSenha(request.Senha);

        // Buscar parceiro se código foi informado
        Guid? parceiroId = null;
        if (!string.IsNullOrWhiteSpace(request.CodigoParceiro))
        {
            var parceiro = await _unitOfWork.Parceiros
                .FindSingleAsync(p => p.CodigoPublico == request.CodigoParceiro.ToUpperInvariant(), cancellationToken: cancellationToken);
            parceiroId = parceiro?.Id;
        }

        // Criar entidade Candidato
        var candidato = new Candidato(
            nome: request.Nome,
            email: request.Email,
            senhaHash: senhaHash,
            perfilId: PerfilCandidatoId,
            dataNascimento: request.DataNascimento)
        {
            Telefone = request.Telefone,
            NomeSocial = request.NomeSocial,
            ParceiroId = parceiroId
        };

        await _unitOfWork.Candidatos.AddAsync(candidato, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Candidato criado: {Email} | ID: {Id}", request.Email, candidato.Id);

        // TODO: Enviar email de confirmação

        return Result.Ok(new CriarCandidatoResponseDto
        {
            Id = candidato.Id,
            Nome = candidato.Nome,
            Email = candidato.Email,
            Mensagem = "Candidato cadastrado com sucesso! Verifique seu email para confirmar a conta."
        });
    }

    /// <summary>
    /// Confirma o email do candidato
    /// </summary>
    public async Task<Result> ConfirmarEmailAsync(
        ConfirmarEmailRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByTokenConfirmacaoAsync(request.Token, cancellationToken);

        if (candidato == null || candidato.Email != request.Email.ToLowerInvariant().Trim())
        {
            return Result.Fail("TOKEN_INVALIDO");
        }

        candidato.ConfirmarEmail();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Email confirmado para: {Email}", request.Email);

        return Result.Ok();
    }

    /// <summary>
    /// Reenvia email de confirmação
    /// </summary>
    public async Task<Result> ReenviarConfirmacaoEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByEmailAsync(email, cancellationToken);

        if (candidato == null || candidato.Status == StatusUsuarioEnum.Ativo)
        {
            // Não revelar se o email existe
            return Result.Ok();
        }

        candidato.GerarNovoTokenConfirmacao();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Reenviar email de confirmação

        return Result.Ok();
    }

    /// <summary>
    /// Busca candidato por ID
    /// </summary>
    public async Task<Result<CandidatoResponseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdCompletoAsync(id, cancellationToken);

        if (candidato == null)
        {
            return Result.Fail<CandidatoResponseDto>("CANDIDATO_NAO_ENCONTRADO");
        }

        var dto = candidato.Adapt<CandidatoResponseDto>();
        dto.Status = candidato.Status.ToString();
        dto.RealizouBigFive = candidato.RealizouTesteBigFive();

        return Result.Ok(dto);
    }

    /// <summary>
    /// Busca candidato por email
    /// </summary>
    public async Task<Result<CandidatoResponseDto>> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByEmailAsync(email, cancellationToken);

        if (candidato == null)
        {
            return Result.Fail<CandidatoResponseDto>("CANDIDATO_NAO_ENCONTRADO");
        }

        var dto = candidato.Adapt<CandidatoResponseDto>();
        dto.Status = candidato.Status.ToString();

        return Result.Ok(dto);
    }

    /// <summary>
    /// Edita dados do candidato
    /// </summary>
    public async Task<Result<CandidatoResponseDto>> EditarAsync(
        Guid id,
        EditarCandidatoRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdAsync(id, cancellationToken);

        if (candidato == null)
        {
            return Result.Fail<CandidatoResponseDto>("CANDIDATO_NAO_ENCONTRADO");
        }

        candidato.AtualizarDados(
            nome: request.Nome ?? candidato.Nome,
            telefone: request.Telefone ?? candidato.Telefone);

        if (request.NomeSocial != null || request.DataNascimento.HasValue)
        {
            candidato.AtualizarDadosCandidato(
                nomeSocial: request.NomeSocial ?? candidato.NomeSocial,
                dataNascimento: request.DataNascimento ?? candidato.DataNascimento);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = candidato.Adapt<CandidatoResponseDto>();
        dto.Status = candidato.Status.ToString();

        return Result.Ok(dto);
    }

    /// <summary>
    /// Cria ou atualiza o perfil pessoal
    /// </summary>
    public async Task<Result<PerfilPessoalResponseDto>> UpsertPerfilPessoalAsync(
        Guid candidatoId,
        PerfilPessoalUpsertRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdAsync(candidatoId, cancellationToken);
        if (candidato == null)
            return Result.Fail<PerfilPessoalResponseDto>("CANDIDATO_NAO_ENCONTRADO");

        PerfilPessoal perfil;

        if (candidato.PerfilPessoalId.HasValue)
        {
            // Atualizar existente
            perfil = await _unitOfWork.PerfisPessoais.GetByIdAsync(candidato.PerfilPessoalId.Value, cancellationToken);
            if (perfil == null)
                return Result.Fail<PerfilPessoalResponseDto>("PERFIL_NAO_ENCONTRADO");

            // Atualizar propriedades
            AtualizarPerfilPessoal(perfil, request);
            _unitOfWork.PerfisPessoais.Update(perfil);
        }
        else
        {
            // Criar novo
            perfil = new PerfilPessoal(request.SobreMim);
            AtualizarPerfilPessoal(perfil, request);
            await _unitOfWork.PerfisPessoais.AddAsync(perfil, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            candidato.PerfilPessoalId = perfil.Id;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Perfil pessoal atualizado para candidato: {CandidatoId}", candidatoId);

        return Result.Ok(MapearPerfilPessoalResponse(perfil));
    }

    /// <summary>
    /// Cria ou atualiza o perfil profissional
    /// </summary>
    public async Task<Result<PerfilProfissionalResponseDto>> UpsertPerfilProfissionalAsync(
        Guid candidatoId,
        PerfilProfissionalUpsertRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdAsync(candidatoId, cancellationToken);
        if (candidato == null)
            return Result.Fail<PerfilProfissionalResponseDto>("CANDIDATO_NAO_ENCONTRADO");

        PerfilProfissional perfil;

        if (candidato.PerfilProfissionalId.HasValue)
        {
            perfil = await _unitOfWork.PerfisProfissionais.GetByIdAsync(candidato.PerfilProfissionalId.Value, cancellationToken);
            if (perfil == null)
                return Result.Fail<PerfilProfissionalResponseDto>("PERFIL_NAO_ENCONTRADO");
        }
        else
        {
            perfil = new PerfilProfissional(request.DispensaExperienciaProfissional);
            await _unitOfWork.PerfisProfissionais.AddAsync(perfil, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            candidato.PerfilProfissionalId = perfil.Id;
        }

        // Atualizar formações
        if (request.FormacoesAcademicas?.Any() == true)
        {
            // TODO: Remover formações existentes e adicionar novas
            foreach (var formacaoReq in request.FormacoesAcademicas)
            {
                var formacao = new FormacaoAcademica(
                    perfilProfissionalId: perfil.Id,
                    grau: formacaoReq.Grau,
                    areaAtuacao: formacaoReq.AreaAtuacao,
                    dataConclusao: formacaoReq.DataConclusao,
                    concluido: formacaoReq.Concluido);
                perfil.AdicionarFormacaoAcademica(formacao);
            }
        }

        // Atualizar experiências
        if (request.ExperienciasProfissionais?.Any() == true)
        {
            foreach (var expReq in request.ExperienciasProfissionais)
            {
                var experiencia = new ExperienciaProfissional(
                    perfilProfissionalId: perfil.Id,
                    empresa: expReq.Empresa,
                    posicao: expReq.Posicao,
                    dataInicio: expReq.DataInicio,
                    dataFim: expReq.DataFim,
                    empregoAtual: expReq.EmpregoAtual);
                perfil.AdicionarExperienciaProfissional(experiencia);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Perfil profissional atualizado para candidato: {CandidatoId}", candidatoId);

        return Result.Ok(MapearPerfilProfissionalResponse(perfil));
    }

    /// <summary>
    /// Salva o resultado do teste Big Five
    /// </summary>
    public async Task<Result<BigFiveResponseDto>> SalvarBigFiveAsync(
        Guid candidatoId,
        BigFiveRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdAsync(candidatoId, cancellationToken);
        if (candidato == null)
            return Result.Fail<BigFiveResponseDto>("CANDIDATO_NAO_ENCONTRADO");

        // Validar pontuações (0-100)
        if (!ValidarPontuacaoBigFive(request))
            return Result.Fail<BigFiveResponseDto>("PONTUACAO_INVALIDA");

        candidato.SalvarTesteBigFive(
            extroversao: request.Extroversao,
            amabilidade: request.Amabilidade,
            consciencia: request.Consciencia,
            estabilidadeEmocional: request.EstabilidadeEmocional,
            aberturaExperiencia: request.AberturaExperiencia);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Teste Big Five salvo para candidato: {CandidatoId}", candidatoId);

        return Result.Ok(MapearBigFiveResponse(candidato));
    }

    /// <summary>
    /// Obtém o resultado do teste Big Five
    /// </summary>
    public async Task<Result<BigFiveResponseDto>> GetBigFiveAsync(
        Guid candidatoId,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdAsync(candidatoId, cancellationToken);
        if (candidato == null)
            return Result.Fail<BigFiveResponseDto>("CANDIDATO_NAO_ENCONTRADO");

        return Result.Ok(MapearBigFiveResponse(candidato));
    }

    /// <summary>
    /// Obtém o perfil pessoal
    /// </summary>
    public async Task<Result<PerfilPessoalResponseDto>> GetPerfilPessoalAsync(
        Guid candidatoId,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdAsync(candidatoId, cancellationToken);
        if (candidato == null || !candidato.PerfilPessoalId.HasValue)
            return Result.Fail<PerfilPessoalResponseDto>("PERFIL_NAO_ENCONTRADO");

        var perfil = await _unitOfWork.PerfisPessoais.GetByIdAsync(candidato.PerfilPessoalId.Value, cancellationToken);
        if (perfil == null)
            return Result.Fail<PerfilPessoalResponseDto>("PERFIL_NAO_ENCONTRADO");

        return Result.Ok(MapearPerfilPessoalResponse(perfil));
    }

    /// <summary>
    /// Obtém o perfil profissional
    /// </summary>
    public async Task<Result<PerfilProfissionalResponseDto>> GetPerfilProfissionalAsync(
        Guid candidatoId,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdCompletoAsync(candidatoId, cancellationToken);
        if (candidato == null || candidato.PerfilProfissional == null)
            return Result.Fail<PerfilProfissionalResponseDto>("PERFIL_NAO_ENCONTRADO");

        return Result.Ok(MapearPerfilProfissionalResponse(candidato.PerfilProfissional));
    }

    // ==========================================
    // Métodos privados de mapeamento
    // ==========================================

    private static PerfilPessoalResponseDto MapearPerfilPessoalResponse(PerfilPessoal perfil)
    {
        var dto = new PerfilPessoalResponseDto
        {
            Id = perfil.Id,
            CorRaca = perfil.CorRaca,
            Pronome = perfil.Pronome,
            IdentidadeGenero = perfil.IdentidadeGenero,
            OrientacaoSexual = perfil.OrientacaoSexual,
            Cpf = perfil.Cpf,
            Rg = perfil.Rg,
            SobreMim = perfil.SobreMim,
            LocalResidencia = perfil.LocalResidencia,
            AcoesAfirmativas = perfil.AcoesAfirmativas,
            DescricaoPcd = perfil.DescricaoPcd,
            Instagram = perfil.Instagram,
            Facebook = perfil.Facebook,
            Linkedin = perfil.Linkedin
        };

        if (perfil.Endereco != null)
        {
            dto.Endereco = new EnderecoResponseDto
            {
                CEP = perfil.Endereco.CEP,
                Rua = perfil.Endereco.Rua,
                Numero = perfil.Endereco.Numero,
                Bairro = perfil.Endereco.Bairro,
                Cidade = perfil.Endereco.Cidade,
                Estado = perfil.Endereco.Estado,
                Complemento = perfil.Endereco.Complemento,
                Latitude = perfil.Endereco.Latitude,
                Longitude = perfil.Endereco.Longitude
            };
        }

        return dto;
    }

    private static PerfilProfissionalResponseDto MapearPerfilProfissionalResponse(PerfilProfissional perfil)
    {
        return new PerfilProfissionalResponseDto
        {
            Id = perfil.Id,
            DispensaExperienciaProfissional = perfil.DispensaExperienciaProfissional,
            FormacoesAcademicas = perfil.FormacoesAcademicas?
                .Select(f => new FormacaoAcademicaResponseDto
                {
                    Id = f.Id,
                    Grau = f.Grau,
                    AreaAtuacao = f.AreaAtuacao,
                    DataConclusao = f.DataConclusao,
                    Concluido = f.Concluido
                }).ToList(),
            ExperienciasProfissionais = perfil.ExperienciasProfissionais?
                .Select(e => new ExperienciaProfissionalResponseDto
                {
                    Id = e.Id,
                    Empresa = e.Empresa,
                    Posicao = e.Posicao,
                    DataInicio = e.DataInicio,
                    DataFim = e.DataFim,
                    EmpregoAtual = e.EmpregoAtual
                }).ToList(),
            Competencias = perfil.CompetenciasCandidatos?
                .Select(c => new CompetenciaCandidatoResponseDto
                {
                    CompetenciaId = c.CompetenciaId,
                    Nome = c.Competencia?.Nome ?? "",
                    Nivel = c.Nivel.ToString()
                }).ToList()
        };
    }

    private static BigFiveResponseDto MapearBigFiveResponse(Candidato candidato)
    {
        return new BigFiveResponseDto
        {
            Extroversao = candidato.Extroversao ?? 0,
            Amabilidade = candidato.Amabilidade ?? 0,
            Consciencia = candidato.Consciencia ?? 0,
            EstabilidadeEmocional = candidato.EstabilidadeEmocional ?? 0,
            AberturaExperiencia = candidato.AberturaExperiencia ?? 0,
            DataUltimoTeste = candidato.DataUltimoTesteBigFive,
            RealizouTeste = candidato.RealizouTesteBigFive()
        };
    }

    private static void AtualizarPerfilPessoal(PerfilPessoal perfil, PerfilPessoalUpsertRequestDto request)
    {
        // Usar reflection para settar propriedades com private set
        var tipo = typeof(PerfilPessoal);

        tipo.GetProperty("CorRaca")?.SetValue(perfil, request.CorRaca);
        tipo.GetProperty("Pronome")?.SetValue(perfil, request.Pronome);
        tipo.GetProperty("IdentidadeGenero")?.SetValue(perfil, request.IdentidadeGenero);
        tipo.GetProperty("OrientacaoSexual")?.SetValue(perfil, request.OrientacaoSexual);
        tipo.GetProperty("Cpf")?.SetValue(perfil, request.Cpf?.Replace(".", "").Replace("-", ""));
        tipo.GetProperty("Rg")?.SetValue(perfil, request.Rg);
        tipo.GetProperty("SobreMim")?.SetValue(perfil, request.SobreMim);
        tipo.GetProperty("LocalResidencia")?.SetValue(perfil, request.LocalResidencia);
        tipo.GetProperty("AcoesAfirmativas")?.SetValue(perfil, request.AcoesAfirmativas);
        tipo.GetProperty("DescricaoPcd")?.SetValue(perfil, request.DescricaoPcd);
        tipo.GetProperty("Instagram")?.SetValue(perfil, request.Instagram);
        tipo.GetProperty("Facebook")?.SetValue(perfil, request.Facebook);
        tipo.GetProperty("Linkedin")?.SetValue(perfil, request.Linkedin);

        // Atualizar endereço se tiver dados
        if (!string.IsNullOrWhiteSpace(request.CEP) || !string.IsNullOrWhiteSpace(request.Cidade))
        {
            var endereco = Domain.ValueObjects.Endereco.Criar(
                cep: request.CEP,
                rua: request.Rua,
                numero: request.Numero,
                bairro: request.Bairro,
                cidade: request.Cidade,
                estado: request.Estado,
                complemento: request.Complemento,
                latitude: request.Latitude,
                longitude: request.Longitude);

            perfil.AtualizarEndereco(endereco);
        }
    }

    private static bool ValidarPontuacaoBigFive(BigFiveRequestDto request)
    {
        return request.Extroversao >= 0 && request.Extroversao <= 100 &&
               request.Amabilidade >= 0 && request.Amabilidade <= 100 &&
               request.Consciencia >= 0 && request.Consciencia <= 100 &&
               request.EstabilidadeEmocional >= 0 && request.EstabilidadeEmocional <= 100 &&
               request.AberturaExperiencia >= 0 && request.AberturaExperiencia <= 100;
    }
}
