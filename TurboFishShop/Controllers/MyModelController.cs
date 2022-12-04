using Microsoft.AspNetCore.Mvc;
using TurboFishShop.Data;
using TurboFishShop.Models;

namespace TurboFishShop.Controllers
{
    public class MyModelController : Controller
    {
        private ApplicationDBContext db;

        public MyModelController(ApplicationDBContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<MyModel> models = db.MyModels;

            return View(models);
        }

        // GET - Create
        public IActionResult Create()
        {
            return View();
        }

        // POST - Create
        [HttpPost]
        public IActionResult Create(MyModel model)
        {
            if (model == null || model.Name == null)
                return RedirectToAction("Index");
            db.MyModels.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");  //переход на страницу Index
        }

		// GET - Edit
		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var model = db.MyModels.Find(id);

			if (model == null)
			{
				return NotFound();
			}

			return View(model);
		}

		//POST - Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(MyModel model)
		{
			if (ModelState.IsValid) // проверка модели на валидность
			{
				db.MyModels.Update(model);  // !!!
				db.SaveChanges();
				return RedirectToAction("Index"); //переход на страницу Index
			}

			return View(model);
		}

		// GET - Delete
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var model = db.MyModels.Find(id);

			if (model == null)
			{
				return NotFound();
			}

			return View(model);
		}

		//POST - Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? id)
		{
			var model = db.MyModels.Find(id);

			if (model == null)
			{
				return NotFound();
			}

			db.MyModels.Remove(model);
			db.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
