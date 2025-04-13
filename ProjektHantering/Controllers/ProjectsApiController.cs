using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektHantering.Models;
using ProjektHantering.Services;

namespace ProjektHantering.Controllers
{
    [Route("api/projects")]
    [ApiController]
    [Authorize]
    public class ProjectsApiController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsApiController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/projects/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            // Check if the current user owns the project
            if (project.UserId != userId)
            {
                return Forbid();
            }

            return Ok(project);
        }

        // DELETE: api/projects/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            // Check if the current user owns the project
            if (project.UserId != userId)
            {
                return Forbid();
            }

            var result = await _projectService.DeleteProjectAsync(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Failed to delete project");
            }
        }
    }
}