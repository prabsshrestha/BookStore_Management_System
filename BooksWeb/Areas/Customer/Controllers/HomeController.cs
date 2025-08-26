using BooksWeb.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BooksWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : BaseHomeController
    {
        //public override IActionResult Index()
        //{
        //    // Admin custom logic
        //    var result = base.Index(); // still reuse base
        //    return result;
        //}

    }
}
