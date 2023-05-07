using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext _dbContext;
        private readonly ILogger<CustomersProvider> _logger;
        private readonly IMapper _mapper;
        public CustomersProvider(CustomersDbContext dbContext, IMapper mapper, ILogger<CustomersProvider> logger )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            SeedData();
        }

        private void SeedData()
        {
            if (!_dbContext.Customers.Any())
            {

                _dbContext.Customers.AddRange(

                    new Customer { Id = 1, Adress = "20 Elm St.", Name = "Jessica Smith" },
                    new Customer { Id = 2, Adress = "30 Main St.", Name = "John Smith" },
                    new Customer { Id = 3, Adress = "100 10th St.", Name = "William Johnson" }
                    );
                _dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCostumerAsync(int id)
        {
            try
            {
                var customer = await _dbContext.Customers.SingleOrDefaultAsync(c=> c.Id == id);
                if (customer != null)
                {
                    var result = _mapper.Map<Db.Customer, Models.Customer>(customer);
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

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> customers, string ErrorMessage)> GetCostumersAsync()
        {
            try
            {
                var customers = await _dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = _mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
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
