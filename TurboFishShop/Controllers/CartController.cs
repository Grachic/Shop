using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurboFishShop.Data;
using TurboFishShop.Models;
using TurboFishShop.Models.ViewModels;
using TurboFishShop.Utility;

namespace TurboFishShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        ApplicationDBContext db;

		ProductUserViewModel productUserViewModel;

        public CartController(ApplicationDBContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathsManager.SessionCart);

                // хотим получить список товаров, которые находятся в корзине
            }

			List<int> productsIdInCart = cartList.Select(p => p.ProductId).ToList();

			//извлекаем сами продукты по списку id
			IEnumerable<Product> productList = db.Product.Where(p => productsIdInCart.Contains(p.Id));

			return View(productList);
        }

        public IActionResult Remove(int id)
        {
			List<Cart> cartList = new List<Cart>();

			if (HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart).Count() > 0)
			{
				cartList = HttpContext.Session.Get<List<Cart>>(PathsManager.SessionCart);
			}

			cartList.Remove(cartList.Where(p => p.ProductId == id).FirstOrDefault());

            // переназначить сессию
			HttpContext.Session.Set(PathsManager.SessionCart, cartList);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public IActionResult Summary()
        {
			ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;

            // если пользователь вошёл в систему, то объект будет определён
			Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<Cart> cartList = new List<Cart>();

			if (HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart) != null &&
				HttpContext.Session.Get<IEnumerable<Cart>>(PathsManager.SessionCart).Count() > 0)
			{
				cartList = HttpContext.Session.Get<List<Cart>>(PathsManager.SessionCart);
			}

			List<int> productsIdInCart = cartList.Select(p => p.ProductId).ToList();

			IEnumerable<Product> productList = db.Product.Where(p => productsIdInCart.Contains(p.Id));

			productUserViewModel = new ProductUserViewModel()
			{
				ApplicationUser = db.ApplicationUsers.FirstOrDefault(x => x.Id == claim.Value),
				ProductList = productList
			};

			return View(productUserViewModel);
		}

	}
}
