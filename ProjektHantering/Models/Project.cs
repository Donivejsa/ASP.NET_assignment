using System;
using System.ComponentModel.DataAnnotations;

namespace ProjektHantering.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name is required")]
        [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public ProjectStatus Status { get; set; }

        // Relation to user (project owner)
        public string UserId { get; set; }

        // Additional fields from the modern design
        public string ClientName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive number")]
        public decimal Budget { get; set; }

        public enum ProjectStatus
        {
            [Display(Name = "In Progress")]
            InProgress,

            [Display(Name = "Completed")]
            Completed
        }
    }

}