using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmbeddedStockTest.Models
{
	public class Category
	{
        public Category()
        {
            ComponentTypes = new List<ComponentType>();
        }
        public int CategoryId { get; set; }
        public string Name { get; set; }
	    public ICollection<ComponentType> ComponentTypes { get; protected set; }
	}
}