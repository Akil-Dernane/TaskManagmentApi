using Application.Abstractions.Persistence;
using Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.UpdateProject
{
    public class UpdateProjectHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectHandler(
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProjectCommand command, CancellationToken ct)
        {
            var project = await _projectRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("Project not found.");

            var nameExists = await _projectRepository.ExistsByNameAsync(command.Name, ct);

            if (nameExists && !string.Equals(project.Name,command.Name,StringComparison.OrdinalIgnoreCase))
            {
                throw new ConflictException(
                "A project with this name already exists.");
            }

            project.UpdateDetails(command.Name, command.Description);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
