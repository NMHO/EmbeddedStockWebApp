using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EmbeddedStockWebApp.Models
{
    public class OneToMany
    {
        [Key]
        public long OneToManyId { get; set; }
        public long CategoryId { get; set; }
        public long ComponentTypeId { get; set; }        
    }
}
