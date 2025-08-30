using Book.Models;
using BooksWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : BaseHomeController
    {
        public override async Task<IActionResult> Index()
        {
            SetBaseViewData();

            return View("~/Views/Home/Index.cshtml");
        }

    }
}
