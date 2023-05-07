using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController:ControllerBase
    {
        private readonly IOrdersProvider _ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            _ordersProvider = ordersProvider;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrdersAsync(int customerId)
        {
            var result = await _ordersProvider.GetOrdersAsync(customerId);
            if (result.IsSuccess)
            {
                return Ok(result.orders);
            }
            return NotFound();
        }

        //[HttpGet("{orderId}")]
        //public async Task<IActionResult> GetOrderAsync(int orderId)
        //{
        //    var result = await _ordersProvider.GetOrderAsync(orderId);
        //    if (result.IsSuccess)
        //    {
        //        return Ok(result.Order);
        //    }
        //    return NotFound();
        //}
    }
}
