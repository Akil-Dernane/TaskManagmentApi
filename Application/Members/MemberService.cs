using Application.Abstractions.Persistence;
using Application.Exceptions;
using Application.Members.DTOs;
using Domain.Entities;

namespace Application.Members
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MemberService(
            IMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> CreateMemberAsync(CreateMemberDto dto, CancellationToken ct)
        {
            var member = new Member(dto.Email);

            if (await _memberRepository.ExistsByEmailAsync(dto.Email, ct)) throw new ConflictException("A member with this email already exists.");

            await _memberRepository.AddAsync(member, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return member.Id;
        }

        public async Task<MemberDetailsDto> GetMemberDetailsByIdAsync(Guid id, CancellationToken ct)
        {
            return await _memberRepository.GetDetailsByIdAsync(id, ct) ?? throw new KeyNotFoundException("Member not found.");
        }

        public async Task UpdateMemberAsync(Guid id, UpdateMemberDto dto, CancellationToken ct)
        {
            var member = await _memberRepository.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Member not found");

            if (await _memberRepository.ExistsByEmailAsync(dto.Email, ct) 
                && !string.Equals(member.Email,dto.Email,StringComparison.OrdinalIgnoreCase))
            {
                throw new ConflictException("A member with this email already exists.");
            }

            member.UpdateEmail(dto.Email);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
