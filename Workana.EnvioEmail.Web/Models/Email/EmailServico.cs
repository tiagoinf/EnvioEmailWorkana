using System.Net.Mail;

namespace Workana.EnvioEmail.Web.Models.Email
{
    public class EmailServico(IConfiguration _configuracao, IWebHostEnvironment _hostingEnvironment) : IEmailServico
    {
        public async Task EnviarEmailBemVindoAsync(string email, string nome)
        {
            var textoTemplate = await BuscarTextoTemplate(ETipoTemplate.EmailBemVindo);

            textoTemplate = textoTemplate.Replace("[data]", DateTime.Now.ToLongDateString());
            textoTemplate = textoTemplate.Replace("[nome]", nome);

            var mensagem = new Mensagem([email], "Vem vindo à Workana", textoTemplate);

            var mensagemEmail = MontarMensagem(mensagem);

            await EnviarEmailAsync(mensagemEmail);
        }

        private async Task<string> BuscarTextoTemplate (ETipoTemplate tipo)
        {
            var caminhoArquivo = Path.Combine(_hostingEnvironment.WebRootPath, "arquivos\\templates", $"{tipo}.html");

            return await EmailTemplate.BuscarTemplate(caminhoArquivo);
        }

        private MailMessage MontarMensagem(Mensagem mensagem)
        {
            MailMessage mensagemEmail = new();

            mensagemEmail.From = new MailAddress(_configuracao["Email:Remetente"] ?? "", _configuracao["Email:NomeRemetente"]);

            foreach (var destinatario in mensagem.Destinatarios)
            {
                mensagemEmail.To.Add(destinatario);
            }

            mensagemEmail.Subject = mensagem.Assunto;
            mensagemEmail.Body = mensagem.Corpo;
            mensagemEmail.IsBodyHtml = true;

            return mensagemEmail;
        }

        private async Task EnviarEmailAsync(MailMessage mensagem)
        {
            using var clienteSmtp = new SmtpClient(_configuracao["Email:Host"], Convert.ToInt32(_configuracao["Email:Porta"]));

            clienteSmtp.EnableSsl = Convert.ToBoolean(_configuracao["Email:SslHabilitado"]);

            clienteSmtp.Credentials = new System.Net.NetworkCredential(_configuracao["Email:Usuario"], _configuracao["Email:Senha"]);

            await clienteSmtp.SendMailAsync(mensagem);
        }
    }
}
