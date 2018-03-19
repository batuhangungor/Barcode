using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Barcode.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int StorageId { get; set; }
        public int BrandId { get; set; }
        public DateTime CreatedDate { get; set; }


        public Storage Storage { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
    }
}
