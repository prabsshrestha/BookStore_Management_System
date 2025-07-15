using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Book.Models.ViewModels;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProduct = _unitofWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objProduct);
        }

        public IActionResult Upsert(int? id) 
        { 
            ProductViewModel productVM = new()
            {
                CategoryList = _unitofWork.Category
				.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString(),
				}),
                Product = new Product()
            };
            if(id == null || id == 0)
            {
                //create
				return View(productVM);
			}
            else
            {
                //update
                productVM.Product = _unitofWork.Product.Get(u => u.Id == id);  
                return View(productVM);
            }

		}

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productVM, IFormFile? file)
        {
            if (ModelState.IsValid) 
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image 
                        var oldimagepath = 
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimagepath))
                        {
                            System.IO.File.Delete(oldimagepath);
                        }
                    }
					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}
                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;
				}
                if(productVM.Product.Id == 0)
                {
					_unitofWork.Product.Add(productVM.Product);
				}
                else
                {
					_unitofWork.Product.Update(productVM.Product);
				}
				_unitofWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");   
            }
            else
            {
                productVM.CategoryList = _unitofWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });
				return View(productVM);
			}
		}

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product product = _unitofWork.Product.Get(u => u.Id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitofWork.Product.Update(obj);
        //        _unitofWork.Save();
        //        TempData["success"] = "Product updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

   //     public IActionResult Delete(int? id)
   //     {
   //         if (id == null || id == 0)
   //         {
   //             return NotFound();
   //         }
			////Product product = _unitofWork.Product.Get(u => u.Id == id);
			//ProductViewModel productVM = new()
			//{
			//	Product = _unitofWork.Product.Get(u => u.Id == id),
			//	CategoryList = _unitofWork.Category
			//	.GetAll().Select(u => new SelectListItem
			//	{
			//		Text = u.Name,
			//		Value = u.Id.ToString(),
			//	})
			//};

			//if (productVM == null)
   //         {
   //             return NotFound();
   //         }
   //         return View(productVM);
   //     }

   //     [HttpPost, ActionName("Delete")]
   //     public IActionResult DeletePost(int? id)
   //     {
			//string wwwRootPath = _webHostEnvironment.WebRootPath;
			//var product = _unitofWork.Product.Get(u => u.Id == id);
   //         if(product == null)
   //         {
   //             return NoContent();
   //         }

   //         if(!string.IsNullOrEmpty(product.ImageUrl)) 
   //         {
			//	var imagepath =
	  //                  Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
   //             if (System.IO.File.Exists(imagepath))
   //             {
   //                 System.IO.File.Delete(imagepath);
   //             }
   //         }
			//_unitofWork.Product.Remove(product);
			//_unitofWork.Save();
			//TempData["success"] = "Product Deleted successfully";
   //         return RedirectToAction("Index");
   //     }

        #region APICalls

        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> objProduct = _unitofWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProduct});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitofWork.Product.Get(u => u.Id==id);
            if(product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldimagepath =
                            Path.Combine(_webHostEnvironment.WebRootPath, 
                            product.ImageUrl.TrimStart('\\'));                             
            if (System.IO.File.Exists(oldimagepath))
            {
                System.IO.File.Delete(oldimagepath);
            }
            _unitofWork.Product.Remove(product);
            _unitofWork.Save();
            return Json(new { success = true, message = "Deleted Successful" });
        }   


        #endregion
    }
}
