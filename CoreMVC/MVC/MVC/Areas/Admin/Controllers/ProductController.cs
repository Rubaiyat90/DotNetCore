using Models;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> pdtList = _unitOfWork.Product.GetAll().ToList();
            return View(pdtList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                CategoryList = _unitOfWork.Category.GetAll().Select
                (u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
                return View();
            }
            
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? pdt = _unitOfWork.Product.Get(u => u.Id == id);
            if (pdt == null)
            {
                return NotFound();
            }
            return View(pdt);
        }
        [HttpPost]
        public IActionResult Edit(Product pdt)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(pdt);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
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
            Product? pdt = _unitOfWork.Product.Get(u => u.Id == id);
            if (pdt == null)
            {
                return NotFound();
            }
            return View(pdt);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? pdt = _unitOfWork.Product.Get(u => u.Id == id);
            if (pdt == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(pdt);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
