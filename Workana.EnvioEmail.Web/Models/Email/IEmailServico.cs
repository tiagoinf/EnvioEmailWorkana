namespace Workana.EnvioEmail.Web.Models.Email
{
    public interface IEmailServico
    {
        Task EnviarEmailBemVindoAsync(string email, string nome);
    }
}
