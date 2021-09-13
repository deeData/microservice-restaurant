using Mango.Services.Email.DbContexts;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        //DbContext must be available as a Singleton for the repository because this is required by the
        //AzureServiceBus- because that instance will be calling the repository
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;
        public EmailRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendAndLogEmail(UpdatePaymentResultMessage message)
        {
            //implement an email sender or call some other class library
            EmailLog emailLog = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully."
            };

            await using var _db = new ApplicationDbContext(_dbContext);
            _db.EmailLogs.Add(emailLog);
            await _db.SaveChangesAsync();
        }
    }
}
