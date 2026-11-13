using Application.Abstractions.Persistence;
using Application.Members.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly TaskManagementDbContext _dbContext;
        public MemberRepository(TaskManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Member member, CancellationToken ct)
        {
            await _dbContext.Members.AddAsync(member,ct);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
        {
            var normalizedEmail = email.ToLower();
            return await _dbContext.Members.AnyAsync(m => m.Email.ToLower() == normalizedEmail,ct);

        }

        public async Task<Member?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Id == id,ct);
        }

        public async Task<MemberDetailsDto?> GetDetailsByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.Members
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new MemberDetailsDto(m.Id, m.Email))
                .FirstOrDefaultAsync(ct);
        }
    }
}
