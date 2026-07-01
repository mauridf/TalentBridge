namespace TalentBridge.Application.DTOs.Dashboard;

/// <summary>
/// Resposta do dashboard administrativo
/// </summary>
public class DashboardAdminResponseDto
{
    // Convites
    public int TotalConvitesEnviados { get; set; }
    public int ConvitesAceitos { get; set; }
    public int ConvitesPendentes { get; set; }
    public double TaxaConversaoConvites { get; set; }

    // Usuários
    public int TotalCandidatos { get; set; }
    public int TotalCandidatosConfirmados { get; set; }
    public int TotalEmpresas { get; set; }
    public int TotalRecrutadores { get; set; }

    // Vagas e Candidaturas
    public int TotalVagasAtivas { get; set; }
    public int TotalCandidaturas { get; set; }
    public int TotalContratacoes { get; set; }

    // Financeiro
    public int TotalPedidos { get; set; }
    public decimal FaturamentoTotal { get; set; }
    public decimal FaturamentoMes { get; set; }
    public int TotalParceiros { get; set; }

    // Sistema
    public string StatusApi { get; set; } = "Online";
    public string StatusBanco { get; set; } = "Online";
    public DateTime UltimaAtualizacao { get; set; } = DateTime.UtcNow;
}
