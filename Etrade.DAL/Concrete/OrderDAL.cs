using Etrade.Business.Concrete;
using Etrade.DAL.Abstract;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.DAL.Concrete
{
    public class OrderDAL : Repository<EtradeDbContext, Order>, IOrderDAL
    {
    }
}
