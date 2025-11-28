using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using E_wallet.Domain.IHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_wallet.Domain.Entities;


namespace E_wallet.Infrastrucure.Helpers
{
    public  class MailingHelper : IEmailHelper
    {
        private   readonly IConfiguration _configuration;
        public MailingHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  async Task SendOtpEmailAsync    (string email, string otp, string UserName)
        {
            string apiKey = _configuration.GetValue<string>("apiKeySendgrid");
            
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ahmadraslan406@outlook.com", "E-Wallet");
            var subject = "Sending otp.";
            var to = new EmailAddress(email, UserName);
            var plainTextContent = $"the otp for verification is {otp}.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent,"");

            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendEmailAsync(string email,string EmailSubject ,string EmailContent, string UserName )
        {
            string apiKey = _configuration.GetValue<string>("apiKeySendgrid");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ahmadraslan406@outlook.com","E-Wallet");
            var subject = EmailSubject;
            var to = new EmailAddress(email, UserName);
            var plainTextContent = EmailContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, "");

            var response = await client.SendEmailAsync(msg);

        }

       
    }
}
