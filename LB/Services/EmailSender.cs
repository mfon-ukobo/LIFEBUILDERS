using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Services
{
	public class EmailSettings
	{
		public string MailServer { get; set; }
		public int MailPort { get; set; }
		public string SenderName { get; set; }
		//public string Sender { get; set; }
		//public string Password { get; set; }
	}

	public interface IEmailSender
	{
		Task<bool> SendEmailAsync(string from, string email, string subject, string message, string username, string password);
	}

	public class EmailSender : IEmailSender
	{
		private readonly EmailSettings emailSettings;
		private readonly IHostingEnvironment env;
		public string Error;

		public EmailSender(IOptions<EmailSettings> emailSettings, IHostingEnvironment env)
		{
			this.emailSettings = emailSettings.Value;
			this.env = env;
		}

		public async Task<bool> SendEmailAsync(string from, string email, string subject, string message, string username, string password)
		{
			try
			{
				var mimeMessage = new MimeMessage();

				mimeMessage.From.Add(new MailboxAddress(emailSettings.SenderName, from));

				mimeMessage.To.Add(new MailboxAddress(email));

				mimeMessage.Subject = subject;

				mimeMessage.Body = new TextPart(TextFormat.Html)
				{
					Text = message
				};

				//mimeMessage.Date = DateTimeOffset.Now;

				//mimeMessage.Sender = new MailboxAddress(from);

				using (var client = new SmtpClient())
				{
					// For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
					client.ServerCertificateValidationCallback = (s, c, h, e) => true;

					foreach (var part in mimeMessage.BodyParts.OfType<TextPart>())
						part.ContentTransferEncoding = ContentEncoding.QuotedPrintable;

					if (env.IsDevelopment())
					{
						// The third parameter is useSSL (true if the client should make an SSL-wrapped
						// connection to the server; otherwise, false).
						await client.ConnectAsync(emailSettings.MailServer, emailSettings.MailPort, true);
					}
					else
					{
						await client.ConnectAsync(emailSettings.MailServer);
					}

					// Note: only needed if the SMTP server requires authentication
					await client.AuthenticateAsync(from, password);

					await client.SendAsync(mimeMessage);

					await client.DisconnectAsync(true);
				}

				return true;
			}
			catch (Exception ex)
			{
				// TODO: handle exception
				Error = ex.Message;
				return false;
				//throw new InvalidOperationException(ex.Message);
			}
		}
	}
}
