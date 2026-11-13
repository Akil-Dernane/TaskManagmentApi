
namespace Application.Projects.UpdateProject
{
    public record UpdateProjectCommand(
        Guid Id,
        string Name,
        string Description
    );
}
