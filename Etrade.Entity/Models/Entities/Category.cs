using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Entity.Models.Entities
{
    public class Category : Entity
    {
        public Category() : base()
        {

        }
        public Category(string name) : base(name)
        {

        }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
