using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class ParametrosGeraisService : IParametrosGeraisService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ParametrosGeraisService> _logger;

    public ParametrosGeraisService(IUnitOfWork unitOfWork, ILogger<ParametrosGeraisService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultadoDto<ParametroResponseDto>> Obter(string chave)
    {
        _logger.LogInformation("Obtendo parâmetro: {Chave}", chave);

        var chaveNormalizada = chave.ToUpperInvariant().Trim();
        var parametro = await _unitOfWork.ParametrosGerais.FindSingleAsync(p => p.Chave == chaveNormalizada);

        if (parametro == null)
            return ResultadoDto<ParametroResponseDto>.Falha("PARAMETRO_NAO_ENCONTRADO", $"Parâmetro '{chave}' não encontrado.");

        return ResultadoDto<ParametroResponseDto>.Ok(new ParametroResponseDto
        {
            Chave = parametro.Chave,
            Valor = parametro.Valor,
            Descricao = parametro.Descricao
        });
    }

    public async Task<ResultadoDto<List<ParametroResponseDto>>> ListarTodos()
    {
        _logger.LogInformation("Listando todos os parâmetros.");

        var parametros = await _unitOfWork.ParametrosGerais.GetAllAsync();

        var result = parametros.Select(p => new ParametroResponseDto
        {
            Chave = p.Chave,
            Valor = p.Valor,
            Descricao = p.Descricao
        }).ToList();

        return ResultadoDto<List<ParametroResponseDto>>.Ok(result);
    }

    public async Task<ResultadoDto<bool>> Atualizar(string chave, string valor)
    {
        _logger.LogInformation("Atualizando parâmetro {Chave} = {Valor}", chave, valor);

        var chaveNormalizada = chave.ToUpperInvariant().Trim();
        var parametro = await _unitOfWork.ParametrosGerais.FindSingleAsync(p => p.Chave == chaveNormalizada);

        if (parametro == null)
        {
            var novoParametro = new ParametrosGerais(chaveNormalizada, valor);
            await _unitOfWork.ParametrosGerais.AddAsync(novoParametro);
        }
        else
        {
            parametro.AtualizarValor(valor);
        }

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Parâmetro {Chave} atualizado.", chave);

        return ResultadoDto<bool>.Ok(true);
    }
}
