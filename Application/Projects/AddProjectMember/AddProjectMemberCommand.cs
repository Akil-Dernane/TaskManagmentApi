using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.AddProjectMember
{
    public record AddProjectMemberCommand(
        Guid ProjectId,
        Guid MemberId,
        ProjectRole Role);
}
