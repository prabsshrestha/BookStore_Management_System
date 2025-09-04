using Microsoft.AspNetCore.Mvc;

namespace BooksWeb.Controllers
{
    public class BaseHomeController : Controller
    {
        protected string GetCurrentUserName()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "Name");
            return claim?.Value ?? "Guest";
        }

        protected void SetBaseViewData()
        {
            string userRole = User.Identity.IsAuthenticated
                ? (User.IsInRole("Admin") ? "Admin" : "Customer")
                : "Guest";

            ViewData["DisplayName"] = GetCurrentUserName();
            ViewData["Role"] = userRole;
        }

        public virtual async Task<IActionResult> Index()
        {
            SetBaseViewData();
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin"});
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });

                }
            }
            return View("~/Views/Home/Index.cshtml");
        }


        public virtual IActionResult Privacy()
        {
            return View("~/Views/Home/Privacy.cshtml");
        }
    }
}
