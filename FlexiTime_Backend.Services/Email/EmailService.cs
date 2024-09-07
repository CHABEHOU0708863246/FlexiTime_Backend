using FlexiTime_Backend.Domain.Configurations;
using FlexiTime_Backend.Domain.Models.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FlexiTime_Backend.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOption _emailOption;
        protected readonly Func<SmtpClient> _smtpClientFactory;

        public EmailService(IOptions<EmailOption> emailOption, Func<SmtpClient> smtpClientFactory = null)
        {
            _emailOption = emailOption.Value;
            _smtpClientFactory = smtpClientFactory ?? (() => new SmtpClient(_emailOption.Host, _emailOption.Port));
        }

        #region SendEmailAsync Method
        public async Task<bool> SendEmailAsync(EmailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var client = _smtpClientFactory();
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailOption.UserName, _emailOption.Password);

                using var message = new MailMessage(from: _emailOption.UserName, to: request.To, subject: request.Subject, body: request.Body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message, cancellationToken);
                return true;
            }
            catch (SmtpException ex)
            {
                throw new InvalidOperationException("Impossible d’envoyer le courriel.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur inattendue s’est produite lors de l’envoi du courrier électronique.", ex);
            }
        }
        #endregion

        #region GeneratePasswordResetEmailContent Method
        // Méthode qui génère le contenu HTML pour un email de réinitialisation de mot de passe.
        // Le lien de réinitialisation est passé en paramètre et inséré dans le corps du message.
        public string GeneratePasswordResetEmailContent(string path)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Réinitialisation de votre mot de passe</title>
            </head>
            <body>
                <div style=""font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; color: #333;"">
                    <h2 style=""color: #4A90E2;"">Bonjour,</h2>
                    <p>Vous avez oublié votre mot de passe et souhaitez le réinitialiser.</p>
                    <p>Veuillez cliquer sur le lien suivant pour en choisir un nouveau:</p>
                    <p><a href=""{path}"" target=""_blank"" rel=""noopener noreferrer"" style=""background-color: #4A90E2; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;"">Réinitialisation de votre mot de passe</a></p>
                    <p>Cordialement,</p>
                </div>
            </body>
            </html>";
        }
        #endregion
    }
}
