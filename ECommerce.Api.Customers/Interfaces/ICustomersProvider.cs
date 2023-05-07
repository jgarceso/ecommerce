using ECommerce.Api.Customers.Models;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Customer> customers, string ErrorMessage)> GetCostumersAsync();
        Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> GetCostumerAsync(int id);
    }
}
