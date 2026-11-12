using Application.Abstractions;

namespace Infrastructure.Services
{
    internal class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
