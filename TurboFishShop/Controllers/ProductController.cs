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

        public ProductController(ApplicationDBContext db)
        {
            this.db = db;
        }
        
        // GET INDEX
        public IActionResult Index()
		{
            IEnumerable<Product> objList =  db.Product;

            // получаем ссылки на сущности категорий
            foreach (var item in objList)
            {
                item.Category = db.Categories.FirstOrDefault(x => x.Id == item.CategoryId);
            }

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

                return View();
            }
            
        }
    }
}
