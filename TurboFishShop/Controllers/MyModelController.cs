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
    }
}
