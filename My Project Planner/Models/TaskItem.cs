using System;
namespace My_Project_Planner.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string? Description { get; set; }

        public double EstimatedHours { get; set; }

        // Foreign Key: Task belongs to one Project
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
