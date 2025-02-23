using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace BookMovieCatalog.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Това е тестова услуга (mock service). Замени я с реална, ако е нужно.
            Console.WriteLine($"Изпращане на имейл до: {email} | Тема: {subject}");
            return Task.CompletedTask;
        }
    }
}
