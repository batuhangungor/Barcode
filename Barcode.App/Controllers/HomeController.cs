using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Barcode.Data.Context;
using Barcode.Data.Repository;
using Barcode.Models;

namespace Barcode.App.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Product> db;
        public HomeController(IRepository<Product> _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var model = db.GetAll();
            return View(model);
        }
    }
}