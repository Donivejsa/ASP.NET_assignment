using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjektHantering.Models;
using ProjektHantering.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjektHantering.Pages.Projects
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IProjectService _projectService;

        public DetailsModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public Project Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Project = await _projectService.GetProjectByIdAsync(id);

            if (Project == null)
            {
                return NotFound();
            }

            // Kontrollera att användaren äger projektet
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Project.UserId != userId)
            {
                return Forbid();
            }

            return Page();
        }
    }
}