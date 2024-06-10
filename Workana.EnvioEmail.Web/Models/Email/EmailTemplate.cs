namespace Workana.EnvioEmail.Web.Models.Email
{
    public class EmailTemplate
    {
        public EmailTemplate()
        {

        }

        public static async Task<string> BuscarTemplate(string caminhoArquivoTemplate)
        {
            var template = await File.ReadAllTextAsync(caminhoArquivoTemplate);

            return template;
        }
    }
}
