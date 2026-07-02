using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public interface IFeedService
{
    Task<ResultadoDto<string>> GerarFeedXml();
}
