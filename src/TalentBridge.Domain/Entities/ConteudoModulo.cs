namespace TalentBridge.Domain.Entities;

public class ConteudoModulo : BaseEntity
{
    public Guid ModuloCursoId { get; private set; }
    public ModuloCurso ModuloCurso { get; private set; } = null!;

    public string Titulo { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public string? UrlVideo { get; private set; }
    public string? UrlMaterial { get; private set; }
    public int Ordem { get; private set; }
    public int DuracaoMinutos { get; private set; }

    protected ConteudoModulo() { }

    public ConteudoModulo(Guid moduloCursoId, string titulo, int ordem, int duracaoMinutos, string? descricao = null, string? urlVideo = null, string? urlMaterial = null)
    {
        ModuloCursoId = moduloCursoId;
        Titulo = titulo;
        Ordem = ordem;
        DuracaoMinutos = duracaoMinutos;
        Descricao = descricao;
        UrlVideo = urlVideo;
        UrlMaterial = urlMaterial;
    }
}
