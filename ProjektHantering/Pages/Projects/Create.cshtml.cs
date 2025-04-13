using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjektHantering.Models;
using ProjektHantering.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjektHantering.Pages.Projects
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IProjectService _projectService;

        public CreateModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [BindProperty]
        public Project Project { get; set; } = new Project();

        public IActionResult OnGet()
        {
            // Sätt startdatum till dagens datum som standard
            Project.StartDate = DateTime.Today;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Sätt användar-ID till inloggad användare
            Project.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _projectService.CreateProjectAsync(Project);

            return RedirectToPage("./Index");
        }
    }
}