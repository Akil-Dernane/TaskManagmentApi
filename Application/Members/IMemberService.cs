using Application.Members.DTOs;

namespace Application.Members
{
    public interface IMemberService
    {
        Task<Guid> CreateMemberAsync(CreateMemberDto dto, CancellationToken ct);

        Task UpdateMemberAsync(Guid id, UpdateMemberDto dto, CancellationToken ct);

        Task<MemberDetailsDto> GetMemberDetailsByIdAsync(Guid id, CancellationToken ct);
    }
}
