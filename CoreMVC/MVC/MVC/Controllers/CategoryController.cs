using Microsoft.AspNetCore.Mvc;
using MVC.Data;
using MVC.Models;

namespace MVC.Controllers
{
    public class CategoryController : Controller
    {
        public readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> ctrList = _db.Categories.ToList();
            return View(ctrList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category ctr)
        {
            if (ctr.Name == ctr.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and Display order cannot be same.");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(ctr);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? ctr = _db.Categories.Find(id);
            if (ctr == null)
            {
                return NotFound();
            }
            return View(ctr);
        }
        [HttpPost]
        public IActionResult Edit(Category ctr)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(ctr);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
