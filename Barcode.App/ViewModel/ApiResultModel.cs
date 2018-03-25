using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barcode.App.ViewModel
{
    public class ApiResultModel
    {
        public string message { get; set; }
        public dynamic data { get; set; }
        public List<ApiError> errors { get; set; }
    }
}
