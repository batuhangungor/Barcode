using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Barcode.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Marka Adı Boş Bırakılamaz!")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
