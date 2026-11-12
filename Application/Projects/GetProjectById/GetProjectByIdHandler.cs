
using Application.Abstractions.Persistence;

namespace Application.Projects.GetProjectById
{
    public class GetProjectByIdHandler(IProjectRepository projectRepository)
    {
        public async Task<ProjectDetailsDto> Handle(
            GetProjectByIdQuery query,
            CancellationToken ct)
        {
            var project = await projectRepository.GetByIdAsync(query.Id,ct ) ?? throw new KeyNotFoundException("Project not found.");

            return project;
        }
    }
}
