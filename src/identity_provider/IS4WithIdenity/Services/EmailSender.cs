using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace IS4WithIdenity.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Task.CompletedTask;
         //   throw new NotImplementedException();
        }
    }
}
