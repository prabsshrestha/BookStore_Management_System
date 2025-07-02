using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        //constructor Injection
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCateory = _db.Categories.ToList();
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
                _db.Categories.Add(obj);
                _db.SaveChanges();
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
            Category? categoryDb = _db.Categories.FirstOrDefault(c => c.Id == id);
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
				_db.Categories.Update(obj);
				_db.SaveChanges();
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
			Category? categoryDb = _db.Categories.FirstOrDefault(c => c.Id == id);
			if (categoryDb == null)
			{
				return NotFound();
			}
			return View(categoryDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
            Category? obj = _db.Categories.Find(id);
            if (obj == null) { 
                return NotFound();
            }
            _db.Categories.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");
		}

        //To Check the Name is unique
        public IActionResult IsNameUnique(string name)
        {
            bool isexist = _db.Categories.Any(c => c.Name == name);
            if (isexist)
            {
                return Json($"The name '{name}' is already in use.");
            }
            return Json(true);
        }
	}
}
