using System;
using System.Collections.Generic;
using System.Text;

namespace Barcode.Models
{
    public class Storage
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
