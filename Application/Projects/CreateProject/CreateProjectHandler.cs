
using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Projects.CreateProject
{
    public class CreateProjectHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;
        public CreateProjectHandler(
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork,
            IClock clock)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public async Task<Guid> Handle(CreateProjectCommand command, CancellationToken ct)
        {
            if (await _projectRepository.ExistsByNameAsync(command.Name, ct))
                throw new ConflictException("A project with this name already exists.");

            var now = _clock.UtcNow;
            var project = new Project(command.Name, command.Description, now);
            await _projectRepository.AddAsync(project, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return project.Id;
        }
    }
}
