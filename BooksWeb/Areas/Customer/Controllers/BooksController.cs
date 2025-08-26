using System.Diagnostics;
using Book.DataAccess.Repository;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc;
namespace BooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(ILogger<BooksController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productlist = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productlist);
        }

        public IActionResult Details(int productid)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == productid, includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
