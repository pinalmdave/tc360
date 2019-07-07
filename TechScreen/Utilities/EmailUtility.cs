using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using TechScreen.ViewModels;

namespace TechScreen.Utilities
{
    public static class EmailUtility
    {
      public static async Task SendGridMessage(SendGridMessage msg)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);

            //var msg = new SendGridMessage();

            //msg.SetFrom(new EmailAddress(emailViewModel.FromEmail, "TechScreen360 Support"));

            //List<EmailAddress> lstSendGridEmailAddresses = new List<EmailAddress>();

            //foreach(var recipient in emailViewModel.lstEmailRecipient)
            //{
            //    var emailAddress = new EmailAddress(recipient.RecipientEmail, recipient.RecipientName);
            //    lstSendGridEmailAddresses.Add(emailAddress);
            //}

            //msg.AddTos(lstSendGridEmailAddresses);

            //msg.SetSubject(emailViewModel.Subject);


            ////msg.AddContent(MimeType.Text, "Hello World plain text!");
            //msg.AddContent(MimeType.Html, "<p>Hello World!</p>");

            await client.SendEmailAsync(msg);
        }
    }
}
