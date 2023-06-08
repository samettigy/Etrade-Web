using Etrade.Entity.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Entity.Models.ViewModels
{
    public class ListViewModel
    {
        public ListViewModel()  // veritabanında işi olmayan classlar için kullanılır update database yapmaya gerek yok.
        {
            Categories = new List<Category>();
            Products = new List<Product>();
        }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
