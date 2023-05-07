using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext _dbContext;
        private readonly ILogger<OrdersProvider> _logger;
        private readonly IMapper _mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if(!_dbContext.Orders.Any()) {
                _dbContext.Orders.AddRange(
                    new Order
                    {
                        CustomerId = 1,
                        Id = 1,
                        OrderDate = DateTime.Today,
                        Items = new List<OrderItem>
                        {
                            new OrderItem {Id = 1,OrderId = 1, ProductId = 1, Quantity= 2, UnitPrice=20}
                        }
                    },
                     new Order
                     {
                         CustomerId = 3,
                         Id = 2,
                         OrderDate = DateTime.Today,
                         Items = new List<OrderItem>
                        {
                            new OrderItem {Id = 2,OrderId = 2, ProductId = 3, Quantity= 1, UnitPrice=150},
                            new OrderItem {Id = 3,OrderId = 2, ProductId = 2, Quantity= 2, UnitPrice=5}
                        }
                     }
                    );
                _dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> GetOrderAsync(int orderId)
        {
            
            try
            {
                var order = await _dbContext.Orders.Where(x => x.Id == orderId).
                    Include(y => y.Items).FirstOrDefaultAsync();
                if (order != null)
                {
                    var result = _mapper.Map<Db.Order, Models.Order>(order);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
           
            try
            {
                var orders = await _dbContext.Orders.Where(x=>x.CustomerId == customerId).
                    Include(y => y.Items).ToListAsync();

                if (orders != null)
                {
                    var result = _mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
