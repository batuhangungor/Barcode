using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Barcode.Data.UnitOfWork;
using Barcode.Models;

namespace Barcode.App.Controllers
{
    public class ProductManagementController : BaseController
    {
        private readonly IUnitOfWork db;

        public ProductManagementController(IUnitOfWork _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Categories()
        {
            var model = db.GetRepository<Category>().GetAll();

            //db.Commit(); -> database aktarımı.
            return View(model);
        }
    }
}