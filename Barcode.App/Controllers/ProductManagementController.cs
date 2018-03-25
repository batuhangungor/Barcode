using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Barcode.Data.UnitOfWork;
using Barcode.Models;
using Microsoft.AspNetCore.Http;
using Barcode.App.ViewModel;
using Barcode.App.ViewModel.CategoryViewModels;

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
            return View(model);
        }

        [HttpPost]
        public IActionResult AddCategory(AddCategoryViewModel model)
        {
            var resultModel = new ApiResultModel();
            int code;
            if (!ModelState.IsValid) // uygun değilse
            {
                code = StatusCodes.Status406NotAcceptable;
                resultModel.message = "Lütfen gerekli alanları doldurunuz!";
                resultModel.errors = new List<ApiError>();
                foreach (var field in ModelState)
                {
                    foreach (var error in field.Value.Errors)
                    {
                        resultModel.errors.Add(new ApiError() { field = field.Key, errorMessage = error.ErrorMessage });
                    }
                }
            }
            else
            {
                var categoryRespoitory = db.GetRepository<Category>();
                try
                {
                    if (categoryRespoitory.Get(q => q.Name == model.categoryName).Any())
                    {
                        code = StatusCodes.Status400BadRequest;
                        resultModel.message = "İlgili kategori zaten eklenmiş!";
                    }
                    else
                    {
                        var category = new Category()
                        {
                            Name = model.categoryName,
                            IsVisible = model.categoryIsVisible
                        };
                        categoryRespoitory.Add(category);
                        db.Commit();
                        code = StatusCodes.Status201Created;
                        resultModel.message = "Kategori ekleme işlemi başarılı";
                        resultModel.data = category;
                    }
                }
                catch (Exception e)
                {
                    code = StatusCodes.Status400BadRequest;
                    resultModel.message = "Kategori eklenirken bir hata oluştu!";
                }
            }
            return StatusCode(code, resultModel);
        }
    }
}