using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Barcode.Data.Context;
using Barcode.Data.Repository;
using Barcode.Models;
using Barcode.Data.UnitOfWork;

namespace Barcode.App.Controllers
{
    public class HomeController : BaseController
    {
        private IUnitOfWork db;
        public HomeController(IUnitOfWork _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var model = db.GetRepository<Product>().GetAll();
            return View(model);
        }
    }
}