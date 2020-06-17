using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB
{
	public class EmailAddress
	{
		public string Name { get; set; }
		public string Address { get; set; }
	}

	public class EmailResult
	{
		public bool Status { get; set; }
		public string Message { get; set; }
	}

	public class EmailMessage
	{
		public EmailMessage()
		{
			
		}

		public List<EmailAddress> ToAddresses { get; set; }
		public List<EmailAddress> FromAddresses { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
	}

	public interface IEmailConfiguration
	{
		string SmtpServer { get; }
		int SmtpPort { get; }
		string SmtpUsername { get; set; }
		string SmtpPassword { get; set; }

		string PopServer { get; }
		int PopPort { get; }
		string PopUsername { get; }
		string PopPassword { get; }
	}

	public class EmailConfiguration : IEmailConfiguration
	{
		public string SmtpServer { get; set; }
		public int SmtpPort { get; set; }
		public string SmtpUsername { get; set; }
		public string SmtpPassword { get; set; }

		public string PopServer { get; set; }
		public int PopPort { get; set; }
		public string PopUsername { get; set; }
		public string PopPassword { get; set; }
	}

	public interface IEmailService
	{
		Task<EmailResult> SendAsync(EmailMessage emailMessage);
		List<EmailMessage> RecieveEmail(int maxCount = 10);
		List<MimeMessage> GetEmails(int maxCount = 10);
	}

	public class EmailService : IEmailService
	{
		private readonly IEmailConfiguration _emailConfiguration;

		public EmailService(IEmailConfiguration emailConfiguration)
		{
			_emailConfiguration = emailConfiguration;
		}

		public List<EmailMessage> RecieveEmail(int maxCount = 10)
		{
			using (var emailClient = new Pop3Client())
			{
				emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, false);

				emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

				emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

				List<EmailMessage> emails = new List<EmailMessage>();
				for (int i = 0; i < emailClient.Count && i < maxCount; i++)
				{
					var message = emailClient.GetMessage(i);
					if (message != null)
					{
						var emailMessage = new EmailMessage
						{
							Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
							Subject = message.Subject
						};
						//emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
						emailMessage.FromAddresses?.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
						emails.Add(emailMessage);
					}
				}

				return emails;
			}
		}

		public async Task<EmailResult> SendAsync(EmailMessage emailMessage)
		{
			try
			{
				var message = new MimeMessage();
				message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
				message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

				message.Subject = emailMessage.Subject;
				//We will say we are sending HTML. But there are options for plaintext etc. 
				message.Body = new TextPart(TextFormat.Html)
				{
					Text = emailMessage.Content
				};

				//Be careful that the SmtpClient class is the one from Mailkit not the framework!
				using (var emailClient = new SmtpClient())
				{
					//The last parameter here is to use SSL (Which you should!)
					await emailClient.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false);

					//Remove any OAuth functionality as we won't be using it. 
					emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

					emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

					emailClient.Send(message);

					emailClient.Disconnect(true);
				}

				return new EmailResult { Status = true, Message = "Success" };
			}
			catch (Exception ex)
			{
				return new EmailResult { Status = true, Message = ex.StackTrace };
			}
		}

		public List<MimeMessage> GetEmails(int maxCount = 10)
		{
			using (var emailClient = new Pop3Client())
			{
				emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, false);

				emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

				emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

				List<MimeMessage> emails = new List<MimeMessage>();
				
				for (int i = 0; i < emailClient.Count; i++)
				{
					var message = emailClient.GetMessage(i);
					emails.Add(message);
				}

				return emails;
			}
		}
	}
}
