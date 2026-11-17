using Domain.Enums;
using System.Data;


namespace Domain.Entities
{
    public class ProjectMembership
    {
        public Guid Id { get;}
        public Guid ProjectId { get;}
        public Guid MemberId { get;}
        public ProjectRole Role { get; private set; }
        public DateTime JoinedAtUtc { get;}


        public ProjectMembership(Guid projectId, Guid memberId, ProjectRole role, DateTime joinedAtUtc)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentException("ProjectId is required.", nameof(projectId));

            if (memberId == Guid.Empty)
                throw new ArgumentException("MemberId is required.", nameof(memberId));

            if (!Enum.IsDefined(role))
                throw new ArgumentException("Invalid project role.", nameof(role));

            if (joinedAtUtc.Kind != DateTimeKind.Utc)
                throw new ArgumentException("JoinedAtUtc must be UTC.", nameof(joinedAtUtc));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            MemberId = memberId;
            Role = role;
            JoinedAtUtc = joinedAtUtc;
        }

        public void ChangeRole(ProjectRole newRole)
        {
            if (!Enum.IsDefined(newRole))
                throw new ArgumentException("Invalid project role.", nameof(newRole));

            Role = newRole;
        }
    }
}
