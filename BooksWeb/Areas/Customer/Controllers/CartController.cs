using Book.DataAccess;
using Book.DataAccess.Data;
using Book.Models;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _db.Cart
                .Include(c => c.Product)
                .ThenInclude(p => p.Category)
                .Where(c => c.ApplicationUserId == userId)
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cartItem = _db.Cart.FirstOrDefault(c => c.Id == id);
            if (cartItem != null)
            {
                _db.Cart.Remove(cartItem);
                _db.SaveChanges();
            }
            return Json(new { success = true, message = "Item removed from cart." });
        }

        [HttpPost]
        public IActionResult BookNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _db.Cart.Where(c => c.ApplicationUserId == userId);
            _db.Cart.RemoveRange(cartItems);
            _db.SaveChanges();

            return Json(new { success = true, message = "Books booked successfully!" });
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
