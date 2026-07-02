using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class FiltroService : IFiltroService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FiltroService> _logger;

    public FiltroService(IUnitOfWork unitOfWork, ILogger<FiltroService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultadoDto<FiltrosResponseDto>> ObterFiltros()
    {
        _logger.LogInformation("Obtendo filtros disponíveis.");

        var vagas = await _unitOfWork.Vagas.GetAllAsync();
        var vagasList = vagas.ToList();

        var dominios = await _unitOfWork.Dominios.GetAllAsync();
        var dominiosList = dominios.ToList();

        var cidades = vagasList
            .Where(v => !string.IsNullOrWhiteSpace(v.Cidade))
            .Select(v => v.Cidade)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        var cargos = vagasList
            .Where(v => !string.IsNullOrWhiteSpace(v.Cargo))
            .Select(v => v.Cargo)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        var regimes = dominiosList
            .Where(d => d.Tipo == Domain.Enums.TipoDominioEnum.RegimeTrabalho)
            .Select(d => d.Descricao)
            .ToList();

        var tiposContratacao = dominiosList
            .Where(d => d.Tipo == Domain.Enums.TipoDominioEnum.TipoContratacao)
            .Select(d => d.Descricao)
            .ToList();

        return ResultadoDto<FiltrosResponseDto>.Ok(new FiltrosResponseDto
        {
            Cidades = cidades,
            Cargos = cargos,
            Regimes = regimes,
            TiposContratacao = tiposContratacao
        });
    }
}
