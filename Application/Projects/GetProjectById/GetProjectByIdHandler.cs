
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;

namespace Application.Projects.GetProjectById
{
    public class GetProjectByIdHandler(IProjectRepository projectRepository,
    IProjectAuthorizationService projectAuthorizationService)
    {
        public async Task<ProjectDetailsDto> Handle(
            GetProjectByIdQuery query,
            CancellationToken ct)
        {
            var project = await projectRepository.GetDetailsByIdAsync(query.Id,ct ) ?? throw new KeyNotFoundException("Project not found.");

            if (!await projectAuthorizationService.IsMemberAsync(query.Id, ct))
                throw new ForbiddenAccessException();

            return project;
        }
    }
}
