using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        ////constructor Injection
        //public CategoryController(ApplicationDbContext db)
        //{
        //    _db = db;
        //}
        private readonly IUnitOfWork _unitofWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //List<Category> objCateory = _db.Categories.ToList();
            List<Category> objCateory = _unitofWork.Category.GetAll().ToList();
            return View(objCateory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order can't exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                //_db.Categories.Add(obj);
                _unitofWork.Category.Add(obj);
                //_db.SaveChanges();
                _unitofWork.Save();
                TempData["success"] = "Category created successfully";
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
            Category? categoryDb = _unitofWork.Category.Get(u => u.Id == id);
            //Category? categoryDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            //Category? categoruDb = _db.Categories.Find(id);
            //Category? categoryDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryDb == null)
            {
                return NotFound();
            }
            return View(categoryDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                //_db.Categories.Update(obj);
                //_db.SaveChanges();
                _unitofWork.Category.Update(obj);
                _unitofWork.Save();
                TempData["success"] = "Category updated successfully";
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
            //Category? categoryDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            Category? categoryDb = _unitofWork.Category.Get(c => c.Id == id);
            if (categoryDb == null)
            {
                return NotFound();
            }
            return View(categoryDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            //Category? obj = _db.Categories.Find(id);
            Category? obj = _unitofWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            //         _db.Categories.Remove(obj);
            //_db.SaveChanges();
            _unitofWork.Category.Remove(obj);
            _unitofWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }

        //To Check the Name is unique
        public IActionResult IsNameUnique(string name, int id)
        {
            //bool isexist = _db.Categories.Any(c => c.Name == name);
            var isexist = _unitofWork.Category.Get(u => u.Name == name);
            if (isexist != null && isexist.Id != id)
            {
                return Json($"The name '{name}' is already in use.");
            }
            return Json(true);
        }
    }
}
