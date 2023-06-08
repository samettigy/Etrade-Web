using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Entity.Models.Entities
{
    public class Entity
    {
        public Entity()
        {
            
        }

        public Entity(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [StringLength(100,MinimumLength =3)]
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
    }
}
