using Models;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModel;


namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            List<Product> pdtList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
            return View(pdtList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if(id==null || id==0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM ,IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                if(file!=null)
                {
                    string filename = Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldProductPath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldProductPath))
                        {
                            System.IO.File.Delete(oldProductPath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + filename;
                }
                if(productVM.Product.Id==0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product created successfully";

                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
            
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
