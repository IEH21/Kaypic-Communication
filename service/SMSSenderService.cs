using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Web3_kaypic.Settings;

namespace Web3_kaypic.service
{
    public class SMSSenderService : ISMSSenderService
    {
        private readonly TwilioSettings _twilioSettings;

        public SMSSenderService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
        }

        public async Task SendSmsAsync(string number, string message)
        {
            // Twilio non configuré → pas de crash
            if (string.IsNullOrWhiteSpace(_twilioSettings.AccountSId) ||
                string.IsNullOrWhiteSpace(_twilioSettings.AuthToken) ||
                string.IsNullOrWhiteSpace(_twilioSettings.FromPhoneNumber))
            {
                return;
            }

            TwilioClient.Init(_twilioSettings.AccountSId, _twilioSettings.AuthToken);

            await MessageResource.CreateAsync(
                to: number,
                from: _twilioSettings.FromPhoneNumber,
                body: message
            );
        }
    }
}