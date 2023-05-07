namespace ECommerce.Api.Orders.Db
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total => Items.Sum(x => x.Quantity * x.UnitPrice);
        public List<OrderItem> Items { get; set; } = new List<OrderItem> { };
    }
}
