using Etrade.Core.Models;
using Etrade.DAL.Abstract;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Etrade.Core.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly EtradeDbContext db;
        private readonly IProductDAL productDAL;
        private readonly ICategoryDAL categoryDAL;
        public ProductController(EtradeDbContext db, IProductDAL productDAL, ICategoryDAL categoryDAL)
        {
            this.db = db;
            this.productDAL = productDAL;
            this.categoryDAL = categoryDAL;
        }

        public IActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products);
        }
        [Authorize(Roles = "Admin,Create")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(categoryDAL.GetAll(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {               
                productDAL.Add(product);
                return RedirectToAction("Index");
            }
            return View();
        }
        [Authorize(Roles = "Admin,Edit")]
        public IActionResult Edit(int id)
        {
            ViewData["CategoryId"] = new SelectList(categoryDAL.GetAll(), "Id", "Name");
            var product = productDAL.Get(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {           
                productDAL.Update(product);
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Details(int id)
        {
            var product = productDAL.Get(id);
            product.Category = categoryDAL.Get(product.CategoryId);
            return View(product);
        }
        [Authorize(Roles = "Admin,Delete")]
        public IActionResult Delete(int id)
        {
            var product = productDAL.Get(id);
            product.Category = categoryDAL.Get(product.CategoryId);
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            productDAL.Delete(product);
            return RedirectToAction("Index");
        }
    }
}
