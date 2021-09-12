using Mango.Services.OrderAPI.DbContexts;
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        //DbContext must be available as a Singleton for the repository because this is required by the
        //AzureServiceBus- because that instance will be calling the repository
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;
        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            //need to create new instance
            await using var _db = new ApplicationDbContext(_dbContext);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int OrderHeaderId, bool paid)
        {
            //need to create new instance
            await using var _db = new ApplicationDbContext(_dbContext);
            var orderHeaderFromDB = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == OrderHeaderId);
            if (orderHeaderFromDB!=null)
            {
                orderHeaderFromDB.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }
            

        }
    }
}
