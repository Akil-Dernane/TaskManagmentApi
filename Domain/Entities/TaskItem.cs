using Domain.Enums;

namespace Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get;}
        public Guid ProjectId { get;}
        public Guid? AssignedMemberId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }

        public ProjectTaskStatus Status { get; private set; }
        public ProjectTaskPriority Priority { get; private set; }

        public DateOnly? DueDate { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public DateTime CreatedAt { get;}
        public DateTime? UpdatedAt { get; private set; }

        public TaskItem(string title, DateTime createdAt, Guid projectId, string? description = null, DateOnly? dueDate = null)
        {
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            EnsureUtc(createdAt, nameof(createdAt));

            if (dueDate.HasValue && dueDate.Value < DateOnly.FromDateTime(createdAt))
            {
                throw new ArgumentException(
                    "DueDate cannot be earlier than CreatedAt.",
                    nameof(dueDate));
            }

            if (projectId == Guid.Empty)
                throw new ArgumentException("ProjectId is required",nameof(projectId));

            Id = Guid.NewGuid();
            Title = title;
            CreatedAt = createdAt;
            ProjectId = projectId;
            Description = description;
            DueDate = dueDate;
            Status = ProjectTaskStatus.Todo;
            Priority = ProjectTaskPriority.Medium;
        }
        public void UpdateDetails(string title, string? description, DateOnly? dueDate, DateTime updatedAt)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(title));

            EnsureUtc(updatedAt, nameof(updatedAt));

            Title = title;
            Description = description;
            DueDate = dueDate;
            UpdatedAt = updatedAt;
        }
        public void Start(DateTime updatedAt)
        {
            if (Status != ProjectTaskStatus.Todo)
                throw new InvalidOperationException($"Cannot change status from {Status} to {ProjectTaskStatus.InProgress}");

            EnsureUtc(updatedAt, nameof(updatedAt));

            Status = ProjectTaskStatus.InProgress;
            UpdatedAt = updatedAt;
        }
        public void Complete(DateTime completedAt)
        {
            if (Status != ProjectTaskStatus.InProgress)
                throw new InvalidOperationException($"Cannot change status from {Status} to {ProjectTaskStatus.Completed}");

            EnsureUtc(completedAt, nameof(completedAt));

            if (completedAt < CreatedAt)
                throw new ArgumentException(
                    "CompletedAt cannot be earlier than CreatedAt.",
                    nameof(completedAt));

            Status = ProjectTaskStatus.Completed;
            CompletedAt = completedAt;
            UpdatedAt = completedAt;
        }
        public void Reopen(DateTime updatedAt)
        {
            if (Status == ProjectTaskStatus.Todo)
                throw new InvalidOperationException($"Cannot change status from {Status} to {ProjectTaskStatus.Todo}");

            EnsureUtc(updatedAt, nameof(updatedAt));

            Status = ProjectTaskStatus.Todo;
            CompletedAt = null;
            UpdatedAt = updatedAt;
        }
        public void ChangePriority(ProjectTaskPriority priority, DateTime updatedAt)
        {
            if (!Enum.IsDefined(priority))
                throw new ArgumentOutOfRangeException(nameof(priority), "Invalid task priority.");

            if (Status == ProjectTaskStatus.Completed)
                throw new InvalidOperationException("Can't change the priority of a completed task");

            EnsureUtc(updatedAt,nameof(updatedAt));

            Priority = priority;
            UpdatedAt = updatedAt;
        }

        public void Assign(Guid memberId, DateTime updatedAt)
        {
            if (memberId == Guid.Empty)
                throw new ArgumentException("MemberId is required", nameof(memberId));

            if (AssignedMemberId is not null)
                throw new InvalidOperationException("The task is already assigned.");

            if(Status == ProjectTaskStatus.Completed)
                throw new InvalidOperationException("Cannot assign a member to a completed task.");

            EnsureUtc(updatedAt, nameof(updatedAt));

            AssignedMemberId = memberId;
            UpdatedAt = updatedAt;
        }
        public void Unassign(DateTime updatedAt)
        {
            if (AssignedMemberId is null)
                throw new InvalidOperationException("The task is already unassigned");

            EnsureUtc(updatedAt, nameof(updatedAt));

            AssignedMemberId = null;
            UpdatedAt = updatedAt;
        }
        private static void EnsureUtc(DateTime dateTime, string paramName)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
                throw new ArgumentException($"{paramName} must be UTC.", paramName);
        }
    }
}
