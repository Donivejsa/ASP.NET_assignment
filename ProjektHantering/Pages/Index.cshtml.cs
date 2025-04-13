using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjektHantering.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Om användaren är inloggad, omdirigera till projektssidan
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Projects/Index");
            }

            // Annars, omdirigera till inloggningssidan
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}