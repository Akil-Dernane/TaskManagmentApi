using Domain.Enums;

namespace Domain.Entities
{
    public class Project
    {
        public Guid Id { get;}
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime CreatedAtUtc { get; private set; }
        public ProjectStatus Status { get; private set; }


        public Project(string name,string description, DateTime createdAtUtc)
        {

            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description)) {
                throw new ArgumentException("Description is required.", nameof(description));
            }

            if (createdAtUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("CreatedAt must be Utc.", nameof(createdAtUtc));
            }

            Id = Guid.NewGuid();
            Status = ProjectStatus.Active;
            Name = name;
            Description = description;
            CreatedAtUtc = createdAtUtc;
        }

        public void UpdateDetails(string name, string description)
        {
            if (Status == ProjectStatus.Archived)
                throw new InvalidOperationException("Can't update Archived projects.");

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description is required.", nameof(description));
            }

            Name = name;
            Description = description;
        }

        public void Archive()
        {
            Status = ProjectStatus.Archived;
        }
    }
}
