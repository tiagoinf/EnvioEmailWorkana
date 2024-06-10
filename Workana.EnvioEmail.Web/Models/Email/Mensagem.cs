using System.Net.Mail;

namespace Workana.EnvioEmail.Web.Models.Email
{
    public class Mensagem
    {
        public List<MailAddress> Destinatarios { get; private set; }

        public string Assunto { get; private set; }

        public string Corpo { get; private set; }

        public Mensagem(IEnumerable<string> destinatarios, string assunto, string corpo)
        {
            Destinatarios = [.. destinatarios.Select(x => new MailAddress(x))];

            Assunto = assunto;
            Corpo = corpo;
        }
    }
}
