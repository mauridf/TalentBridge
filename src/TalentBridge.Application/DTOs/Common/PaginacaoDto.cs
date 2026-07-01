namespace TalentBridge.Application.DTOs.Common;

/// <summary>
/// Requisição de paginação
/// </summary>
public class PaginacaoRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Resposta de paginação
/// </summary>
public class PaginacaoResponseDto<T>
{
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    public MetadadosPaginacaoDto MetaData { get; set; } = new();
}

public class MetadadosPaginacaoDto
{
    public int CurrentPage { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}
