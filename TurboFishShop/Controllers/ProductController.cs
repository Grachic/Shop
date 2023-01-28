﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TurboFishShop_Data;
using TurboFishShop.Models;
using TurboFishShop.Models.ViewModels;
using TurboFishShop_Models;
using TurboFishShop_Utility;

namespace TurboFishShop.Controllers
{
	[Authorize(Roles = PathsManager.AdminRole)]
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
            // получаем лист категорий для отправки его во View
            IEnumerable<SelectListItem> CategoriesList = db.Category.Select(x =>
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
				}),
				MyModelsList = db.MyModels.Select(x =>
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

				if (productViewModel.Product == null)
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
				// AsNoTracking() importamt !
				var product  = db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productViewModel.Product.Id);

				if (files.Count > 0) // юзер загружает другой файл
				{
					string upload = wwwRoot + PathsManager.ImageProductPath;
					string imageName = Guid.NewGuid().ToString();

					string extension = Path.GetExtension(files[0].FileName);
					string path = upload + imageName + extension;

					// delete old file
					var oldFile = upload + product.Image;

					if (System.IO.File.Exists(oldFile))
					{
						System.IO.File.Delete(oldFile);
					}

					// скопируем файл на сервер
					using (var fileStream = new FileStream(path, FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					}

					productViewModel.Product.Image = imageName + extension;
				}
				else // фотка не поменялась
				{
					productViewModel.Product.Image = product.Image;  // оставляем имя прежним
				}

				db.Product.Update(productViewModel.Product);
			}

			db.SaveChanges();

			return RedirectToAction("Index");
		}

		// GET - DELETE
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var product = db.Product.Find(id);
			if (product == null)
			{
				return NotFound();
			}
			product.Category = db.Categories.Find(product.CategoryId);
			product.MyModel = db.MyModels.Find(product.MyModelId);

			return View(product);
		}

		// POST
		[HttpPost]
		public IActionResult DeletePost(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var product = db.Product.Find(id);

			string upload = webHostEnvironment.WebRootPath + PathsManager.ImageProductPath;

			var oldFile = upload + product.Image;

			if (System.IO.File.Exists(oldFile))
				System.IO.File.Delete(oldFile);

			db.Product.Remove(product);
			db.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
