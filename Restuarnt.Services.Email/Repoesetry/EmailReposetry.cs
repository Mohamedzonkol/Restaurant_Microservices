using Microsoft.EntityFrameworkCore;
using Restuarnt.Services.Email.DbContexts;
using Restuarnt.Services.Email.Messages;
using Restuarnt.Services.Email.Models;

namespace Restuarnt.Services.Email.Repoesetry
{
    public class EmailReposetry:IEmailReposetry
    {
        private readonly DbContextOptions<AppDbContext>_context;
        public EmailReposetry(DbContextOptions<AppDbContext> dbContext)
        {
            _context= dbContext;
        }

        public async Task SendAndEmailLog(UpdatePaymentResult message)
        {
            //Implement en Email Sender or Call Some Other Class library
            EmailLog email = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been Created Successfully"
            };
            await using var db = new AppDbContext(_context);
            db.EmailLogs.Add(email);
            await db.SaveChangesAsync();
        }
    }
}
