using System.Net.Mail;

namespace Domain.Entities;

public class Member
{
    public Guid Id { get; }
    public string Email { get; private set; }

    public Member(string email)
    {
        ValidateEmail(email);

        Id = Guid.NewGuid();
        Email = email;
    }

    public void UpdateEmail(string email)
    {
        ValidateEmail(email);
        Email = email;
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        try
        {
            var address = new MailAddress(email);

            if (address.Address != email)
                throw new ArgumentException("Invalid email format.", nameof(email));
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }
    }
}