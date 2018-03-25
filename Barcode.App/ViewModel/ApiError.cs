using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barcode.App.ViewModel
{
    public class ApiError
    {
        public string errorType { get; set; }
        public string errorMessage { get; set; }
        public string field { get; set; }
    }
}
