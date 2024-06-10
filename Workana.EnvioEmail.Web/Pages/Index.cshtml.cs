using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Workana.EnvioEmail.Web.Models.Email;

namespace Workana.EnvioEmail.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IEmailServico _emailServico;

        public IndexModel(ILogger<IndexModel> logger, IEmailServico emailServico)
        {
            _logger = logger;
            _emailServico = emailServico;
        }

        [BindProperty]
        public InputModel Input { get; set; } = null!;

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await _emailServico.EnviarEmailBemVindoAsync(Input.Email, Input.Nome);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro eo enviar email");

                        TempData["Erro"] = "Erro eo enviar email";

                        return Page();
                    }

                    TempData["Sucesso"] = "Email enviado com sucesso";
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao montar mensagem email");

                TempData["Erro"] = "Erro ao montar mensagem email";
            }

            return Page();
        }
    }

    public class InputModel
    {
        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(256, ErrorMessage = "{0} pode ter no máximo {1} caracteres")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "{0} é obrigatório")]
        [MaxLength(256, ErrorMessage = "{0} pode ter no máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "{0} inválido")]
        public string Email { get; set; } = null!;
    }
}
