using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Task
    {
        public Guid Id { get;}
        public Guid ProjectId { get;}
        public string Title { get;}
        public string Description { get;}

        public ProjectTaskStatus Status { get;}
        public ProjectTaskPriority Priority { get;}

        public DateOnly DueDate { get;}
        public DateTime CompletedAt { get; }
        public DateTime CreatedAt { get;}
        public DateTime UpdatedAt { get;}
    }
}
