using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbeddedStockWebApp.Models
{
    public class Category
    {
        public Category()
        {
            ComponentTypes = new List<ComponentType>();
        }

        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<ComponentType> ComponentTypes { get; protected set; }

    }
}
