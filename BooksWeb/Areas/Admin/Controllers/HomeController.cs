using Book.DataAccess.Data;
using BooksWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseHomeController
    {
        private readonly ApplicationDbContext _dbcontext;

        public HomeController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public override async Task<IActionResult> Index()
        {
            SetBaseViewData();
            ViewBag.TotalCategories = await _dbcontext.Categories.CountAsync();
            ViewBag.TotalProduct = await _dbcontext.Products.CountAsync();
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
