using E_wallet.Domain.IHelpers;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class SMSHelper : ISMSHelper
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromNumber;
    private readonly string _messagingServiceSid;

    public SMSHelper(IConfiguration cfg)
    {
        var s = cfg.GetSection("Twilio");
        _accountSid = s["AccountSid"]!;
        _authToken = s["AuthToken"]!;
        _fromNumber = s["FromNumber"];                 // may be null if using service SID
        _messagingServiceSid = s["MessagingServiceSid"]; // may be empty
    }

    public async Task SendSmsAsync(string toPhoneNumber, string message)
    {
        TwilioClient.Init(_accountSid, _authToken);

        
        var options = new CreateMessageOptions(new PhoneNumber(toPhoneNumber))
        {
            Body = message
        };


        if (!string.IsNullOrWhiteSpace(_messagingServiceSid))
            options.MessagingServiceSid = _messagingServiceSid;
        else
            options.From = new PhoneNumber(_fromNumber);   // MUST be set when no service SID

        await MessageResource.CreateAsync(options);
    }
}
