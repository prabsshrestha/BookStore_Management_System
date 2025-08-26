using Microsoft.AspNetCore.Mvc;

namespace BooksWeb.Controllers
{
    public class BaseHomeController : Controller
    {
        public virtual IActionResult Index()
        {
            string userRole = User.IsInRole("Admin") ? "Admin" : "Customer";

            ViewData["Role"] = userRole;
            return View("~/Views/Home/Index.cshtml");
        }

        public virtual IActionResult Privacy()
        {
            return View("~/Views/Home/Privacy.cshtml");
        }
    }
}
