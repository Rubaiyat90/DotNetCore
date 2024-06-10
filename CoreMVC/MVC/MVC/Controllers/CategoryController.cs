using DataAccess.Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;


namespace MVC.Controllers
{
    public class CategoryController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> ctrList = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(ctr);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
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
            Category? ctr = _unitOfWork.Category.Get(u => u.Id == id);
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
                _unitOfWork.Category.Update(ctr);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? ctr = _unitOfWork.Category.Get(u => u.Id == id);
            if (ctr == null)
            {
                return NotFound();
            }
            return View(ctr);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? ctr = _unitOfWork.Category.Get(u => u.Id == id);
            if (ctr == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(ctr);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
