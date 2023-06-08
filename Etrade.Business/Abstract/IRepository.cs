using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Business.Abstract
{
    public interface IRepository<Tentity> where Tentity : class
    {
        void Add(Tentity entity);
        void Update(Tentity entity);
        void Delete(Tentity entity);
        List<Tentity> GetAll(Expression<Func<Tentity,bool>> filter=null); // Home Controller için home index viewinda home index olan bütün ürünleri getirmek için kullanılır.
        Tentity Get(int id);
    }
}
