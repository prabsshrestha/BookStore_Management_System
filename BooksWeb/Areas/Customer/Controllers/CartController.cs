using Book.DataAccess;
using Book.DataAccess.Data;
using Book.Models;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int productId, int count)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please log in to add items to your cart." });
            var cartFromDb = _db.Cart.FirstOrDefault(c => c.ApplicationUserId == userId && c.ProductId == productId);

            if (cartFromDb == null)
            {
                var cart = new Cart
                {
                    ApplicationUserId = userId,
                    ProductId = productId,
                    Count = count
                };
                _db.Cart.Add(cart);
            }
            else
            {
                cartFromDb.Count += count;
            }

            _db.SaveChanges();

            //return RedirectToAction("Details", "Books", new { productid = productId });
            return Json(new { success = true, message = "Item added to cart successfully!" });

        }
    }
}
