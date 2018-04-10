using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Barcode.App.ViewModel.CategoryViewModels
{
    public class UpdateCategoryViewModel
    {
        [Required(ErrorMessage ="Kategori Adı Boş Geçilemez")]
        public string categoryName { get; set; }

        public bool categoryIsVisible { get; set; }

        public int categoryId { get; set; }
    }
}
