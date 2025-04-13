using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjektHantering.Models;
using ProjektHantering.Services;
using static ProjektHantering.Models.Project;

namespace ProjektHantering.Pages.Projects
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IProjectService _projectService;

        public IndexModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public List<Project> Projects { get; set; } = new List<Project>();

        [BindProperty(SupportsGet = true)]
        public string CurrentFilter { get; set; }

        [BindProperty]
        public Project ProjectInput { get; set; } = new Project();

        public int AllCount { get; set; }
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }

        public async Task OnGetAsync(string filter)
        {
            CurrentFilter = filter;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get all user projects to calculate counts
            var allUserProjects = await _projectService.GetUserProjectsAsync(userId);
            AllCount = allUserProjects.Count;
            InProgressCount = allUserProjects.Count(p => p.Status == ProjectStatus.InProgress);
            CompletedCount = allUserProjects.Count(p => p.Status == ProjectStatus.Completed);

            // Filter projects based on the current filter
            if (string.IsNullOrEmpty(filter))
            {
                Projects = allUserProjects;
            }
            else if (filter.ToLower() == "inprogress")
            {
                Projects = allUserProjects.Where(p => p.Status == ProjectStatus.InProgress).ToList();
            }
            else if (filter.ToLower() == "completed")
            {
                Projects = allUserProjects.Where(p => p.Status == ProjectStatus.Completed).ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Sätt användar-ID
                ProjectInput.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Sätt standard startdatum om det inte är angivet
                if (ProjectInput.StartDate == default)
                {
                    ProjectInput.StartDate = DateTime.Today;
                }

                // Sätt status om inte färdig
                if (ProjectInput.Status != ProjectStatus.Completed)
                {
                    ProjectInput.Status = ProjectStatus.InProgress;
                }

                // Skapa projektet
                await _projectService.CreateProjectAsync(ProjectInput);

                // Visa meddelande
                TempData["SuccessMessage"] = "Project created successfully!";

                // Omdirigera för att ladda om sidan med GET
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // Lägg till fel i ModelState
                ModelState.AddModelError(string.Empty, $"Error creating project: {ex.Message}");

                // Ladda om data för sidan
                await OnGetAsync(CurrentFilter);

                return Page();
            }
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string userId, string name, string description, DateTime startDate, DateTime? endDate, ProjectStatus status)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Make sure the project belongs to the current user
            if (userId != currentUserId)
            {
                return Forbid();
            }

            // Get existing project
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Update project properties
            project.Name = name;
            project.Description = description;
            project.StartDate = startDate;
            project.EndDate = endDate;
            project.Status = status;

            // Update the project
            await _projectService.UpdateProjectAsync(project);

            return RedirectToPage();
        }
    }
}