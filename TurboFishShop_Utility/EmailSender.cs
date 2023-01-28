using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;

namespace TurboFishShop_Utility
{
	public class EmailSender : IEmailSender
	{
		private IConfiguration _configuration { get; set; }
		
		// данные отлавливаюся всегда в конструкторе при использовании внедрения зависимостей
		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Execute(email, subject, htmlMessage);
		}
		
		public async Task Execute(string email, string subject, string htmlMessage)
		{
			// получение информации из appsettings.json
			MailJetSettings mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();
			
			MailjetClient client = new MailjetClient(mailJetSettings.ApiKey, mailJetSettings.SecretKey);
			MailjetRequest request = new MailjetRequest
			{
				Resource = Send.Resource,
			};

			var emailMessage = new TransactionalEmailBuilder()
				.WithFrom(new SendContact(PathsManager.EmailSender))
				.WithSubject(subject)
				.WithHtmlPart(htmlMessage)
				.WithTo(new SendContact(email))
				.Build();

			var response = await client.SendTransactionalEmailAsync(emailMessage);
		}
	}
}