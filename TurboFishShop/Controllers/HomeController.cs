using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TurboFishShop_Data;
using TurboFishShop.Models;
using TurboFishShop.Models.ViewModels;
using TurboFishShop_Models;
using TurboFishShop_Models.ViewModels;
using TurboFishShop_Utility;

namespace TurboFishShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDBContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db)
        {
            this.db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Products = db.Product,
                Categories = db.Categories
            };

            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
			List<Cart> cartList = new List<Cart>();

			if (HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart).Count() > 0)
			{
				cartList = HttpContext.Session.Get<List<Cart>>(PathsManager.SessionCart);

			}

			DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                Product = db.Product.Include(p => p.Category).Where(p => p.Id == id).FirstOrDefault(),
                IsInCart = false,
            };

            // проверка на наличие товара в корзине
            // если товар есть, то меняем свойство
			foreach (var item in cartList)
			{
				if (item.ProductId == id)
				{
					detailsViewModel.IsInCart = true;
				}
			}


			return View(detailsViewModel);
        }

		[HttpPost]
		public IActionResult RemoveFromCart(int id)
		{
			List<Cart> cartList = new List<Cart>();
			if (HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart).Count() > 0)
			{
				cartList = HttpContext.Session.Get<List<Cart>>(PathsManager.SessionCart);
			}
			cartList.Remove(cartList.Find(x => x.ProductId == id));

			HttpContext.Session.Set(PathsManager.SessionCart, cartList);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult DetailsPost(int id)
        {
			List<Cart> cartList = new List<Cart>();
            
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart).Count() > 0)
			{
                cartList = HttpContext.Session.Get<List<Cart>>(PathsManager.SessionCart);
			}

			cartList.Add(new Cart { ProductId = id });

			HttpContext.Session.Set(PathsManager.SessionCart, cartList);

			return RedirectToAction("Index");
		}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}