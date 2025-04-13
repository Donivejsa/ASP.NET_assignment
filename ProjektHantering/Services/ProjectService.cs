using Microsoft.EntityFrameworkCore;
using ProjektHantering.Data;
using ProjektHantering.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjektHantering.Models.Project;

namespace ProjektHantering.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<List<Project>> GetProjectsByStatusAsync(ProjectStatus status)
        {
            return await _context.Projects
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            try
            {
                // Kontrollera att projektet inte är null
                if (project == null)
                    throw new ArgumentNullException(nameof(project));

                // Kontrollera att användar-ID är satt
                if (string.IsNullOrEmpty(project.UserId))
                    throw new InvalidOperationException("UserId must be set when creating a project");

                // Lägg till projektet i DbSet
                _context.Projects.Add(project);

                // Spara ändringar till databasen
                await _context.SaveChangesAsync();

                return project;
            }
            catch (Exception ex)
            {
                // Logga felet (i en riktig app skulle du använda en riktig loggare)
                Console.WriteLine($"Error in CreateProjectAsync: {ex.Message}");
                throw; // Kasta om undantaget för att hantera det i anropande kod
            }
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}