using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TurboFishShop.Data;
using TurboFishShop.Models;
using TurboFishShop.Models.ViewModels;

namespace TurboFishShop.Controllers
{
	public class ProductController : Controller
	{
        private ApplicationDBContext db;
        private IWebHostEnvironment webHostEnvironment;

        public ProductController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        
        // GET INDEX
        public IActionResult Index()
		{
            IEnumerable<Product> objList =  db.Product;
            /*
            // получаем ссылки на сущности категорий
            foreach (var item in objList)
            {
                item.Category = db.Categories.FirstOrDefault(x => x.Id == item.CategoryId);
            }
            */
			return View(objList);
		}

        // GET - CreateEdit
        public IActionResult CreateEdit(int? id)
        {
            /*
            // получаем лист категорий для отправки для отправки его во View
            IEnumerable<SelectListItem> CategoriesList = db.Categories.Select(x =>
                new SelectListItem
                {
                    Text = x.Name, 
                    Value = x.Id.ToString()
                });

            // отправляем лист категорий во View
            //ViewBag.CategoriesList = CategoriesList;
            ViewData["CategoriesList"] = CategoriesList;
            */

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoriesList = db.Categories.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null)
            {
                // create product
                return View(productViewModel);
            }
            else
            {
                // edit product
                productViewModel.Product = db.Product.Find(id);
                if (productViewModel == null)
                {
                    return NotFound();
                }

                return View(productViewModel);
            }
            
        }

        // POST - CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit(ProductViewModel productViewModel)
        {
            var files = HttpContext.Request.Form.Files;

            string wwwRoot = webHostEnvironment.WebRootPath;

            if (productViewModel.Product.Id == 0)
            {
                // create
                string upload = wwwRoot + PathsManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();

                string extension = Path.GetExtension(files[0].FileName);

                string path = upload + imageName + extension;

                // скопируем файл на сервер
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productViewModel.Product.Image = imageName + extension;

                db.Product.Add(productViewModel.Product);
            }
            else
            {
                // update
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
