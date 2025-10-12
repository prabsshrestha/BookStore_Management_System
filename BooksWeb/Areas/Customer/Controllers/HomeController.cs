using Book.DataAccess.Data;
using BooksWeb.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : BaseHomeController
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext dbcontext, UserManager<IdentityUser> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        public override async Task<IActionResult> Index()
        {
            SetBaseViewData();
            int totalCart = 0;
            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                totalCart = await _dbcontext.Cart
                    .Where(c => c.ApplicationUserId == userId)
                    .SumAsync(c => c.Count);
            }

            ViewBag.TotalCart = totalCart;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
