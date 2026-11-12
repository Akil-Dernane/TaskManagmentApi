using Application.Abstractions.Persistence;

namespace Infrastructure.Persistence
{
    internal class EfUnitOfWork : IUnitOfWork
    {
        private readonly TaskManagementDbContext _dbContext;

        public EfUnitOfWork(TaskManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _dbContext.SaveChangesAsync(ct);

        }
    }
}
