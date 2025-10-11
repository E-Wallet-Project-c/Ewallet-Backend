using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Infrastrucure.Helpers
{
    public class MailingHelper
    {
        public static async Task SendOtpEmail()
        {
            var apiKey = "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";
            /* var options = new SendGridClientOptions
            {
                ApiKey = apiKey
            };
            options.SetDataResidency("eu"); 
            var client = new SendGridClient(options); */
            // uncomment the above 6 lines if you are sending mail using a regional EU subuser
            // and remove the client declaration just below
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ahmadraslan406@outlook.com", "User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("mohammedsabuabdo@gmail.com", "New usser");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}
