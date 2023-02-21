using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;

namespace BUS.Services
{
	public interface IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string body);
	}

	public class SendEmailService : IEmailSender
	{
		private readonly MailSettings _mailSettings;

		public SendEmailService(IOptions<MailSettings> mailSettings)
		{
			_mailSettings = mailSettings.Value;
		}

		public async Task SendEmailAsync(string email, string subject, string body)
		{
			var message = new MimeMessage
			{
				Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail)
			};
			message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
			message.To.Add(MailboxAddress.Parse(email));
			message.Subject = subject;

			var builder = new BodyBuilder
			{
				HtmlBody = body
			};
			message.Body = builder.ToMessageBody();

			using var smtp = new MailKit.Net.Smtp.SmtpClient();
			try
			{
				await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
				await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);

				await smtp.SendAsync(message);
			}
			catch
			{

			}

			smtp.Disconnect(true);
		}
	}

	public class MailSettings
	{
#nullable disable
		public string DisplayName { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public string Mail { get; set; }
		public string Password { get; set; }
	}
}
