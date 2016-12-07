using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmbeddedStockTest.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}