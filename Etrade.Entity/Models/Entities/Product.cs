using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Entity.Models.Entities
{
    public class Product : Entity
    {
        public Product() : base()
        {

        }

        public Product(string name, Category category, string image, int stock, decimal price, bool isHome) : base(name)
        {
            Category = category;
            Image = image;
            Stock = stock;
            Price = price;
            IsHome = isHome;
        }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        [Range(0,(double)decimal.MaxValue)]
        public decimal Price { get; set; }
        public bool IsHome { get; set; } //Anasayfa görünüp görünmemesi için 

    }
}
