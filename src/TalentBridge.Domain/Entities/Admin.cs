namespace TalentBridge.Domain.Entities;

public class Admin : Usuario
{
    protected Admin() { }

    public Admin(string nome, string email, string senhaHash, Guid perfilId)
        : base(nome, email, senhaHash, perfilId, "Admin")
    {
        Ativar();
    }
}
