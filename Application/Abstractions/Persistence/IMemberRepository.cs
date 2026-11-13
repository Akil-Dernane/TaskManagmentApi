using Application.Members.DTOs;
using Domain.Entities;

namespace Application.Abstractions.Persistence
{
    public interface IMemberRepository
    {
        Task AddAsync(Member member,CancellationToken ct);
        Task<Member?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<MemberDetailsDto?> GetDetailsByIdAsync(Guid id, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
    }
}
