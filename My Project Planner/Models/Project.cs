using System;
using System.Collections.Generic;

namespace My_Project_Planner.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Budget { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<TaskItem> Tasks { get; set; } = new();
    }
}
