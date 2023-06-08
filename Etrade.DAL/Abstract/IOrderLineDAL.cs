using Etrade.Business.Abstract;
using Etrade.Entity.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.DAL.Abstract
{
    public interface IOrderLineDAL : IRepository<OrderLine>
    {
    }
}
