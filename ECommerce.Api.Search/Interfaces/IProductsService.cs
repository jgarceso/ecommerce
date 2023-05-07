using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IProductsService
    {
        Task<(bool isSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync();
    }
}
