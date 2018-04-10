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

        public IActionResult Brands()
        {
            var model = db.GetRepository<Brand>().GetAll();
            return View(model);
        }

        public IActionResult BrandDetails(int Id)
        {
            Brand brand;
            if (Id == 0)
            {
                brand = new Brand();
            }
            else
            {
                brand = db.GetRepository<Brand>().Get(q => q.Id == Id).FirstOrDefault(); // ?? new Brand() değer null'sa ikinci değeri al
                if (brand == null)
                {
                    brand = new Brand();
                }
            }


            return View(brand);
        }

        [HttpPost]
        public IActionResult BrandDetails(Brand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        db.GetRepository<Brand>().Add(model);
                        ViewBag.successMessage = "Marka Ekleme İşlemi Başarılı";
                    }
                    else
                    {
                        Brand brand = db.GetRepository<Brand>().Get(q => q.Id == model.Id).FirstOrDefault();
                        brand.Name = model.Name;
                        ViewBag.successMessage = "Marka Güncelleme İşlemi Başarılı";
                    }
                    db.Commit();
                }

            }
            catch (Exception)
            {
                ViewBag.successMessage = null;
            }
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

        [HttpPost]
        public IActionResult GetCategory(int categoryId)
        {
            var resultModel = new ApiResultModel();
            int code;
            try
            {
                var model = db.GetRepository<Category>().Get(q => q.Id == categoryId).FirstOrDefault();
                if (model == null)
                {
                    code = StatusCodes.Status400BadRequest;
                    resultModel = new ApiResultModel() { message = "Kategori bulunamadı!" };
                }
                else
                {
                    code = StatusCodes.Status200OK;
                    resultModel = new ApiResultModel() { data = model };
                }
            }
            catch (Exception e)
            {
                code = StatusCodes.Status400BadRequest;
                resultModel.message = "Kategori Detayları alınırken hata oluştu!";
            }
            return StatusCode(code, resultModel);

        }

        [HttpPost]
        public IActionResult UpdateCategory(UpdateCategoryViewModel model)
        {
            var resultModel = new ApiResultModel();
            int code;
            try
            {
                if (!ModelState.IsValid)
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
                    var category = db.GetRepository<Category>().Get(q => q.Id == model.categoryId).FirstOrDefault();
                    if (category == null)
                    {
                        code = StatusCodes.Status400BadRequest;
                        resultModel.message = "İlgili kategori bulunamadı!";
                    }
                    else
                    {
                        category.Name = model.categoryName;
                        category.IsVisible = model.categoryIsVisible;
                        db.Commit();
                        code = StatusCodes.Status201Created;
                        resultModel.message = "Kategori güncelleme işlemi başarılı";
                        resultModel.data = category;
                    }
                }
            }
            catch (Exception e)
            {
                code = StatusCodes.Status400BadRequest;
                resultModel.message = "Kategori Detayları alınırken hata oluştu!";
            }
            return StatusCode(code, resultModel);

        }

    }
}