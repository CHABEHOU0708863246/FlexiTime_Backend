using FlexiTime_Backend.Domain.Models.Email;

namespace FlexiTime_Backend.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailRequest request, CancellationToken cancellationToken);

        string GeneratePasswordResetEmailContent(string path);
    }
}
