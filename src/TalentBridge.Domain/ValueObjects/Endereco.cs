namespace TalentBridge.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um endereço completo.
/// É imutável e reutilizado por Empresa, Parceiro, PerfilPessoal e Vaga.
/// </summary>
public class Endereco : IEquatable<Endereco>
{
    public string? CEP { get; }
    public string? Rua { get; }
    public string? Numero { get; }
    public string? Bairro { get; }
    public string? Cidade { get; }
    public string? Estado { get; }
    public string? Complemento { get; }
    public double? Latitude { get; }
    public double? Longitude { get; }

    // Construtor privado para forçar uso do método de criação
    private Endereco(
        string? cep,
        string? rua,
        string? numero,
        string? bairro,
        string? cidade,
        string? estado,
        string? complemento,
        double? latitude,
        double? longitude)
    {
        CEP = cep?.Replace("-", "").Replace(".", "");
        Rua = rua;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado?.ToUpper();
        Complemento = complemento;
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Factory method para criar um novo endereço
    /// </summary>
    public static Endereco Criar(
        string? cep = null,
        string? rua = null,
        string? numero = null,
        string? bairro = null,
        string? cidade = null,
        string? estado = null,
        string? complemento = null,
        double? latitude = null,
        double? longitude = null)
    {
        return new Endereco(cep, rua, numero, bairro, cidade, estado, complemento, latitude, longitude);
    }

    /// <summary>
    /// Retorna o endereço formatado em uma única string
    /// </summary>
    public string? EnderecoCompleto()
    {
        if (string.IsNullOrWhiteSpace(Rua)) return null;

        var endereco = $"{Rua}, {Numero}";
        if (!string.IsNullOrWhiteSpace(Complemento))
            endereco += $" - {Complemento}";
        endereco += $" - {Bairro}";
        endereco += $" - {Cidade}/{Estado}";
        if (!string.IsNullOrWhiteSpace(CEP))
            endereco += $" - CEP: {CEP}";

        return endereco;
    }

    /// <summary>
    /// Verifica se o endereço possui coordenadas geográficas
    /// </summary>
    public bool PossuiCoordenadas() => Latitude.HasValue && Longitude.HasValue;

    /// <summary>
    /// Verifica se o endereço está completo (possui CEP, rua, número, bairro, cidade, estado)
    /// </summary>
    public bool EstaCompleto() =>
        !string.IsNullOrWhiteSpace(CEP) &&
        !string.IsNullOrWhiteSpace(Rua) &&
        !string.IsNullOrWhiteSpace(Numero) &&
        !string.IsNullOrWhiteSpace(Bairro) &&
        !string.IsNullOrWhiteSpace(Cidade) &&
        !string.IsNullOrWhiteSpace(Estado);

    // Implementação de igualdade (Value Objects são comparados por valor)
    public override bool Equals(object? obj) => Equals(obj as Endereco);

    public bool Equals(Endereco? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return CEP == other.CEP &&
               Rua == other.Rua &&
               Numero == other.Numero &&
               Bairro == other.Bairro &&
               Cidade == other.Cidade &&
               Estado == other.Estado &&
               Complemento == other.Complemento &&
               Latitude == other.Latitude &&
               Longitude == other.Longitude;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CEP, Rua, Numero, Bairro, Cidade, Estado, Complemento, Latitude, Longitude);
    }

    public static bool operator ==(Endereco? left, Endereco? right) => Equals(left, right);
    public static bool operator !=(Endereco? left, Endereco? right) => !Equals(left, right);

    public override string ToString() => EnderecoCompleto() ?? "Endereço não informado";
}
