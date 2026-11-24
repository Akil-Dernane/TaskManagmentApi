using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Projects
{
    public class UpdateProjectRequest
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;
    }
}