using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookBuddyFinal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookBuddyFinal.Data;
using System.Security.Claims;

namespace BookBuddyFinal.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationDbContext> _userManager;

        private readonly BookBuddyContext _context;

        public CartController(BookBuddyContext context)
        {
            _context = context;
        }

        // GET: Cart
        public IActionResult Index()
        {
            double shipping=30;
            double total=0;
            double totalPrice=0;
            var cartItems = new List<CartItem>();
            var s = HttpContext.Session.GetString("cartId");
            if (s != null)
            {
                var cartId = int.Parse(s);
                cartItems = _context.CartItem.Include(c=>c.Product).Where(x => x.CartId == cartId).ToList();

                
                foreach (var item in cartItems)
                {
                    total += (item.Product.Price)*item.Quantity;

                }

                shipping = 30 * (cartItems.Count());
                if (shipping > 100)
                {
                    shipping = 100;
                }

                totalPrice = total + shipping;

            }
            ViewBag.total = total;
            ViewBag.totalPrice = totalPrice;
            ViewBag.shipping = shipping;
            return View(cartItems);
        }

        public IActionResult AddToCart(int productId, int quantity = 1,string redirect="yes")
        {
            var s = HttpContext.Session.GetString("cartId");
            if (string.IsNullOrEmpty(s))
            {
                return RedirectToAction("Index", "Products");

            }

            var cartId = int.Parse(s);
            var product = _context.Products.Find(productId);
            var cartItem = new CartItem { ProductId = product.ProductId, Quantity = quantity, CartId = cartId };
            var dupProduct = _context.CartItem.SingleOrDefault(x => x.CartId == cartId && x.ProductId == productId);
            if (dupProduct == null)
            {
                _context.CartItem.Add(cartItem);
            }
            else
            {
                dupProduct.Quantity += 1;
            }

            _context.SaveChanges();
            var items = _context.CartItem.Count(x => x.CartId == cartId);
            if (redirect=="no")
            {
                return RedirectToAction("Index", "Cart");
            }
            
            else
            {
                return RedirectToAction("Index", "Products");
            }
            
        }

        public IActionResult DeleteFromCart(int productId, int quantity = 1)
        {
            var s = HttpContext.Session.GetString("cartId");
            if (string.IsNullOrEmpty(s))
            {
                return RedirectToAction("Index", "Products");

            }

            var cartId = int.Parse(s);
            var product = _context.Products.Find(productId);
            //var cartItem = new CartItem { ProductId = product.ProductId, Quantity = quantity, CartId = cartId };
            var dupProduct = _context.CartItem.SingleOrDefault(x => x.CartId == cartId && x.ProductId == productId);
            if (dupProduct == null)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                if (quantity>1 || dupProduct.Quantity<=1)
                {
                    _context.CartItem.Remove(dupProduct);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Cart");
                }
                else
                {

                    dupProduct.Quantity -= 1;
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Cart");
                }
            }

            
        }

        [Authorize]
        [HttpGet]
        public IActionResult CheckOut()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CheckOut(UsersAddress useraddress)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            useraddress.UserId = userId;
            _context.UsersAddress.Add(useraddress);
            _context.SaveChanges();
            double shipping = 30;
            double total = 0;
            double totalPrice = 0;
            var cartItems = new List<CartItem>();
            var s = HttpContext.Session.GetString("cartId");
            if (s != null)
            {
                var cartId = int.Parse(s);
                cartItems = _context.CartItem.Include(c => c.Product).Where(x => x.CartId == cartId).ToList();


                foreach (var item in cartItems)
                {
                    total += (item.Product.Price) * item.Quantity;

                }

                shipping = 30 * (cartItems.Count());
                if (shipping > 100)
                {
                    shipping = 100;
                }

                totalPrice = total + shipping;

            }
            ViewBag.total = total;
            ViewBag.totalPrice = totalPrice;
            ViewBag.shipping = shipping;
            ViewBag.useraddress = useraddress;
            return View("Payment", cartItems);
        }

        [Authorize]
        public IActionResult Order(int addressId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var s = int.Parse(HttpContext.Session.GetString("cartId"));
            PlacedOrder po = new PlacedOrder { CartId = s, UserId = userId, AddressId = addressId };
            _context.PlacedOrder.Add(po);

            _context.SaveChanges();

            HttpContext.Session.Clear();

            return View();
        }
    }
}
