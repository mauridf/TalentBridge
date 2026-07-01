namespace TalentBridge.Domain.Entities;

/// <summary>
/// Entidade base para todas as entidades do sistema.
/// Fornece Id (Guid), CreatedAt e UpdatedAt.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único da entidade
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Data e hora de criação do registro (UTC)
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Data e hora da última atualização do registro (UTC)
    /// </summary>
    public DateTime UpdatedAt { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza a data de modificação
    /// </summary>
    public void AtualizarDataModificacao()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Define o ID (útil para reconstituir entidades do banco)
    /// </summary>
    public void DefinirId(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(BaseEntity? left, BaseEntity? right) => Equals(left, right);
    public static bool operator !=(BaseEntity? left, BaseEntity? right) => !Equals(left, right);
}
