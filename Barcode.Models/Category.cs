using System;
using System.Collections.Generic;
using System.Text;

namespace Barcode.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
