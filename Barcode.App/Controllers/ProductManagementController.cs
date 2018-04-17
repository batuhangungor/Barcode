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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        //categories

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

        //brands

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

        public IActionResult BrandDelete(int Id)
        {
            try
            {
                Brand BrandDelete = db.GetRepository<Brand>().Get(q => q.Id == Id).FirstOrDefault();
                db.GetRepository<Brand>().Delete(BrandDelete);
                db.Commit();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
            return RedirectToAction("brands");
        }

        //products

        public IActionResult Products(string name, string barcode, int page)
        {
            var pageDisplayCount = 1;
            if (page == 0)
            {
                page = 1;
            }
            var model = db.GetRepository<Product>().Get(q=> (name == null || q.Name.ToLower().Contains(name.ToLower())) &&
                                                            (barcode == null || q.Barcode.ToLower().Contains(barcode.ToLower())))
                                                    .Skip(page - 1 * pageDisplayCount).Take(pageDisplayCount)
                                                    .ToList();
            return View(model);
        }

        public IActionResult ProductDetail(int Id)
        {
            ViewBag.brands = db.GetRepository<Brand>().GetAll().Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            ViewBag.categories = db.GetRepository<Category>().GetAll().Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            Product product = db.GetRepository<Product>().Get(q => q.Id == Id).Include(q=>q.Storage).FirstOrDefault() ?? new Product();
            return View(product);
        }

        [HttpPost]
        public IActionResult ProductDetail(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        model.CreatedDate = DateTime.UtcNow;
                        db.GetRepository<Product>().Add(model);
                        db.Commit();
                        ViewBag.successMessage = "Ürün Ekleme İşlemi Başarılı";
                    }
                    else
                    {
                        var product = db.GetRepository<Product>().Get(q => q.Id == model.Id).Include(q=> q.Storage).FirstOrDefault();
                        product.Name = model.Name;
                        product.Price = model.Price;
                        product.Storage.Count = model.Storage.Count;
                        product.Barcode = model.Barcode;
                        product.BrandId = model.BrandId;
                        product.CategoryId = model.CategoryId;
                        db.Commit();
                        ViewBag.successMessage = "Ürün Güncelleme İşlemi Başarılı";
                    }
                    return View(model);
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }




    }
}