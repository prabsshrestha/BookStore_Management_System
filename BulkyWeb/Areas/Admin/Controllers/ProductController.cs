using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Book.DataAccess.Repository.IRepository;
using Book.Models;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProduct = _unitofWork.Product.GetAll().ToList();
            return View(objProduct);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid) 
            {
                _unitofWork.Product.Add(obj);
                _unitofWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");   
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _unitofWork.Product.Get(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Product.Update(obj);
                _unitofWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _unitofWork.Product.Get(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Product.Remove(obj);
                _unitofWork.Save();
                TempData["success"] = "Product Deleted successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
