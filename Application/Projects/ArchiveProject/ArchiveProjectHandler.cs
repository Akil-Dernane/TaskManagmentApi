
using Application.Abstractions.Persistence;

namespace Application.Projects.ArchiveProject
{
    public class ArchiveProjectHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ArchiveProjectHandler(
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ArchiveProjectCommand command, CancellationToken ct)
        {
            var project = await _projectRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("Project not found.");

            project.Archive();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
