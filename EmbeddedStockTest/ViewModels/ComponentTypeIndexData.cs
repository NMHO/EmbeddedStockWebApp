using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmbeddedStockTest.Models;

namespace EmbeddedStockTest.ViewModels
{
    public class ComponentTypeIndexData
    {
        public IEnumerable<ComponentType> ComponentTypes { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Component> Components { get; set; }
    }
}