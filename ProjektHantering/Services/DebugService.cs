using Microsoft.EntityFrameworkCore;
using ProjektHantering.Data;
using ProjektHantering.Models;
using System;
using System.Threading.Tasks;
using static ProjektHantering.Models.Project;

namespace ProjektHantering.Services
{
    public class DebugService
    {
        private readonly ApplicationDbContext _context;

        public DebugService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TestDatabaseConnection()
        {
            try
            {
                // Testa om vi kan nå databasen
                bool canConnect = await _context.Database.CanConnectAsync();
                return canConnect;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Project> CreateTestProject(string userId)
        {
            var testProject = new Project
            {
                Name = "Test Project " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Description = "Detta är ett automatiskt skapat testprojekt.",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1),
                Status = ProjectStatus.InProgress,
                UserId = userId,
                ClientName = "Test Client",
                Budget = 1000
            };

            _context.Projects.Add(testProject);
            await _context.SaveChangesAsync();
            return testProject;
        }

        public async Task<int> GetProjectCount()
        {
            return await _context.Projects.CountAsync();
        }
    }
}