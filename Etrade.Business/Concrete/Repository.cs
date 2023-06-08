using Etrade.Business.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Business.Concrete
{
    public class Repository<Tcontext, Tentity> : IRepository<Tentity> where Tentity : class where Tcontext : DbContext, new()
    {
        public void Add(Tentity entity)
        {
            using (var db = new Tcontext())
            {
                db.Add(entity);
                //db.Set<Tentity>().Add(entity); 2. yöntem
                //db.Entry(entity).State = EntityState.Added; 3. yöntem
                db.SaveChanges();

            }
        }

        public void Delete(Tentity entity)
        {
            using (var db = new Tcontext())
            {
                db.Remove(entity);
                db.SaveChanges();
            }
        }

        public Tentity Get(int id)
        {
            using (var db = new Tcontext())
            {
                var entity = db.Find<Tentity>(id);
                //var entity = db.Set<Tentity>().Find(id);
                return entity;
            }
        }

        public List<Tentity> GetAll(Expression<Func<Tentity, bool>> filter = null)
        {
            using (var db = new Tcontext())
            {
                return filter == null ? db.Set<Tentity>().ToList() : db.Set<Tentity>().Where(filter).ToList(); // if else burda bu şekilde çalışır. ? işaretinin sağı true : nin sağı false
                //if (filter == null)
                //    return db.Set<Tentity>().ToList();
                //else
                //    return db.Set<Tentity>().Where(filter).ToList();
            }
        }

        public void Update(Tentity entity)
        {
            using (var db = new Tcontext())
            {
                db.Update(entity);
                db.SaveChanges();
            }
        }
    }
}
