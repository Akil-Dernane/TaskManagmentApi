namespace Application.Abstractions.Authentication
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        Guid? MemberId { get; }
        bool IsAuthenticated { get; }
    }
}