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
    }
}
