using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BulkyWebRazor.Pages.Categories
{
	[BindProperties]
    public class CreateModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		//[BindProperty]
		public Category Category { get; set; }
		public CreateModel(ApplicationDbContext db)
		{
			_db = db;
		}
		public void OnGet()
        {
        }

		public IActionResult OnPost()
		{
			_db.Categories.Add(Category);
			_db.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToPage("Index");
		}

		public JsonResult OnGetIsNameUnique(string name)
		{
			bool exists = _db.Categories.Any(c => c.Name == name);
			if (exists)
			{
				return new JsonResult($"The name '{name}' is already in use.");
			}
			return new JsonResult(true);
		}
	}
}
