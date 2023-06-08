using Etrade.DAL.Abstract;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.Core.Controllers
{
    [Authorize] // en düşük yetkilendirme kullanıcı girişidir. 
    public class CategoryController : Controller
    {
        private readonly ICategoryDAL categoryDAL;

        public CategoryController(ICategoryDAL categoryDAL)
        {
            this.categoryDAL = categoryDAL;
        }

        // Default olarak get 
        public IActionResult Index()
        {
            return View(categoryDAL.GetAll());
        }
        [Authorize(Roles = "Admin,Create")] // admin ve create yetkisi olanlar erişebilecek.
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryDAL.Add(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [Authorize(Roles = "Admin,Edit")]
        public IActionResult Edit(int id)
        {
            return View(categoryDAL.Get(id));
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            var model = categoryDAL.Get(category.Id);
            model.Name = category.Name;
            model.Description = category.Description;
            if (ModelState.IsValid)
            {
                categoryDAL.Update(model);
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Details(int id)
        {
            return View(categoryDAL.Get(id));
        }
        [Authorize(Roles = "Admin,Delete")] //admin delete yetkisi olanlar yapabilir.
        public IActionResult Delete(int id)
        {
            return View(categoryDAL.Get(id));
        }
        [HttpPost, ActionName("Delete")] //farklı bir isimde postu açılınca name bu şekilde yazılır.
        public IActionResult DeleteConfirmed(Category category)
        {
            categoryDAL.Delete(category);
            return RedirectToAction("Index");
        }

    }
}
