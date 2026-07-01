namespace TalentBridge.Domain.Entities;

/// <summary>
/// Curso complementar realizado pelo candidato
/// </summary>
public class Curso : BaseEntity
{
    public Guid PerfilProfissionalId { get; private set; }
    public PerfilProfissional PerfilProfissional { get; private set; } = null!;

    public Guid? VagaId { get; private set; }
    public Vaga? Vaga { get; private set; }

    /// <summary>
    /// Nome do curso
    /// </summary>
    public string NomeCurso { get; private set; } = string.Empty;

    /// <summary>
    /// Data de conclusão
    /// </summary>
    public DateTime? DataConclusao { get; private set; }

    /// <summary>
    /// Indica se o curso foi concluído
    /// </summary>
    public bool Concluido { get; private set; }

    protected Curso() { }

    public Curso(Guid perfilProfissionalId, string nomeCurso, DateTime? dataConclusao = null, bool concluido = false, Guid? vagaId = null)
    {
        PerfilProfissionalId = perfilProfissionalId;
        NomeCurso = nomeCurso;
        DataConclusao = dataConclusao;
        Concluido = concluido;
        VagaId = vagaId;
    }
}
