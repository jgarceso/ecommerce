using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ECommerce.Api.Orders.Db
{
    public class OrdersDbContext: DbContext
    {
        public OrdersDbContext(DbContextOptions options) : base(options) { }       
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> Items { get; set;}
    }
}
