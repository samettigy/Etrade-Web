namespace Etrade.Entity.Models.Entities
{
    public class OrderLine
    {
        public OrderLine()
        {
            
        }

        public OrderLine(int quantity, decimal price, Product product)
        {
            Quantity = quantity;
            Price = price;
            Product = product;
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set;}
        public int ProductId { get; set; }  
        public Product Product { get; set; }
    }
}