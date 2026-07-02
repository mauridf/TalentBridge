namespace TalentBridge.Domain.Entities;

/// <summary>
/// Treinamento recomendado para vagas ou candidatos
/// </summary>
public class Treinamento : BaseEntity
{
    public string NomeCurso { get; private set; } = string.Empty;
    public string? Categoria { get; private set; }
    public string? Nivel { get; private set; }
    public string? Descricao { get; private set; }
    public string? ResultadosAprendizagem { get; private set; }
    public string? Criador { get; private set; }
    public double Avaliacao { get; private set; }
    public int QuantidadeAvaliacoes { get; private set; }
    public int DuracaoMinutos { get; private set; }

    // Relacionamento N:N com Vagas
    public ICollection<Vaga> Vagas { get; private set; } = new List<Vaga>();

    // Relacionamento com Competências
    public ICollection<CompetenciaTreinamento> CompetenciasTreinamentos { get; private set; } = new List<CompetenciaTreinamento>();

    // Módulos e conteúdos do curso
    public ICollection<ModuloCurso> ModulosCursos { get; private set; } = new List<ModuloCurso>();

    // Itens incluídos no treinamento
    public ICollection<ItemIncluido> ItensIncluidos { get; private set; } = new List<ItemIncluido>();

    protected Treinamento() { }

    public Treinamento(string nomeCurso, int duracaoMinutos)
    {
        NomeCurso = nomeCurso;
        DuracaoMinutos = duracaoMinutos;
    }
}
