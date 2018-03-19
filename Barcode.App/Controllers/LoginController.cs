using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Barcode.App.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username,string password)
        {
            if (username == "berkan" && password == "123456")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}