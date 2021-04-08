using BookBuddyFinal.Models;
using BookBuddyFinal.Models.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookBuddyFinal.Controllers
{
    public class HomeController : Controller
    {
        readonly BookBuddyContext ctxt = new BookBuddyContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart(int productId,int quantity=1)
        {
            
            var product = ctxt.Products.Find(productId);
            var cartItem = new CartItem { ProductId =product.ProductId,Quantity=quantity,CartId=int.Parse(HttpContext.Session.GetString("cartId"))};
            ctxt.CartItem.Add(cartItem);
            ctxt.SaveChanges();
            var items = ctxt.CartItem.Count(x => x.CartId == int.Parse(HttpContext.Session.GetString("cartId")));
            ViewBag.items = items;
            return RedirectToAction("Index","Products");
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
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
