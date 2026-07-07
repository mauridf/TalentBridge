namespace TalentBridge.Application.DTOs.Segmento;

public class SegmentoResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
