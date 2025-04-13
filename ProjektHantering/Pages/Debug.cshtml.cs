using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjektHantering.Models;
using ProjektHantering.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjektHantering.Pages
{
    [Authorize]
    public class DebugModel : PageModel
    {
        private readonly DebugService _debugService;

        public DebugModel(DebugService debugService)
        {
            _debugService = debugService;
        }

        public bool CanConnect { get; set; }
        public int ProjectCount { get; set; }
        public Project CreatedProject { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                CanConnect = await _debugService.TestDatabaseConnection();
                ProjectCount = await _debugService.GetProjectCount();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                CreatedProject = await _debugService.CreateTestProject(userId);
                CanConnect = true;
                ProjectCount = await _debugService.GetProjectCount();
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                CanConnect = await _debugService.TestDatabaseConnection();
                ProjectCount = await _debugService.GetProjectCount();
                return Page();
            }
        }
    }
}