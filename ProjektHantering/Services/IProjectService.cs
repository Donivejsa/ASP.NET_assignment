using ProjektHantering.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProjektHantering.Models.Project;

namespace ProjektHantering.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<List<Project>> GetProjectsByStatusAsync(ProjectStatus status);
        Task<List<Project>> GetUserProjectsAsync(string userId);
        Task<Project> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int id);
    }
}