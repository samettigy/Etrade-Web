using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Entity.Models.ViewModels
{
    public class OrderStateViewModel
    {
        public int OrderId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
