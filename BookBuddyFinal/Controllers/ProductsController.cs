using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookBuddyFinal.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using BookBuddyFinal.Models.Home;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BookBuddyFinal.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BookBuddyContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;
        private object player;

        public ProductsController(BookBuddyContext context, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        // GET: Products
        public IActionResult Index(string search, int? page)
        {
            //ClaimsPrincipal currentUser = this.User;
            //var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var ci = HttpContext.Session.GetString("cartId");
            if (string.IsNullOrEmpty(ci))
            {
                var cart = new Cart();
                _context.Cart.Add(cart);
                _context.SaveChanges();
                HttpContext.Session.SetString("cartId", cart.CartId.ToString());
            }
            
                HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search, 4, page));
            
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["VendorId"] = new SelectList(_context.Users, "UserId", "UserEmail");
            ViewData["ProductCategory"] = new SelectList(_context.Category, "CategoryId", "CategoryName");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ProductId,VendorId,ProductName,MetaTitle,ProductSummery,ProductDescription,ProductType,Sku,Price,Discount,Quantity,IsAvailable,CreatedAt,UpdatedAt,PublishedAt,StartsAt,EndsAt,ProductPic,ImageFile")] Products products)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileName(products.ImageFile.FileName);
                string pPath = Path.Combine(rootPath + "/Images/", fileName);
                products.ProductPic = fileName;
                var filStream = new FileStream(pPath, FileMode.Create);
                await products.ImageFile.CopyToAsync(filStream);
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VendorId"] = new SelectList(_context.Users, "UserId", "UserEmail", products.VendorId);
            return View(products);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["VendorId"] = new SelectList(_context.Users, "UserId", "UserEmail", products.VendorId);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,VendorId,ProductName,MetaTitle,ProductSummery,ProductDescription,ProductType,Sku,Price,Discount,Quantity,IsAvailable,CreatedAt,UpdatedAt,PublishedAt,StartsAt,EndsAt,ProductPic")] Products products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VendorId"] = new SelectList(_context.Users, "UserId", "UserEmail", products.VendorId);
            return View(products);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
