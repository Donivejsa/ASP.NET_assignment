using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjektHantering.Models;
using ProjektHantering.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using static ProjektHantering.Models.Project;

namespace ProjektHantering.Pages.Projects
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IProjectService _projectService;

        public EditModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [BindProperty]
        public Project Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Project = await _projectService.GetProjectByIdAsync(id);

            if (Project == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Project.UserId != userId)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string userId, string name, string description,
            DateTime startDate, DateTime? endDate, ProjectStatus status, string clientName, decimal budget)
        {
            Console.WriteLine($"OnPostEditAsync called. ID: {id}, Name: {name}, Status: {status}, Budget: {budget}");

            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User ID is missing";
                    return RedirectToPage("./Index");
                }

                if (userId != currentUserId)
                {
                    return Forbid();
                }

                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null)
                {
                    TempData["ErrorMessage"] = $"Project with ID {id} was not found";
                    return RedirectToPage("./Index");
                }

                // Update project properties
                project.Name = name;
                project.Description = description;
                project.StartDate = startDate;
                project.EndDate = endDate;
                project.Status = status;
                project.ClientName = clientName;
                project.Budget = budget;

                // Update the project
                await _projectService.UpdateProjectAsync(project);

                TempData["SuccessMessage"] = "Project updated successfully!";

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnPostEditAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                TempData["ErrorMessage"] = $"Error updating project: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }
    }
}