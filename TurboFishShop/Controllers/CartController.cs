using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurboFishShop_Data;
using TurboFishShop.Models;
using TurboFishShop.Models.ViewModels;
using TurboFishShop_Models;
using TurboFishShop_Models.ViewModels;
using TurboFishShop_Utility;

namespace TurboFishShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        ApplicationDBContext db;

		ProductUserViewModel productUserViewModel;

		IWebHostEnvironment webHostEnvironment;

		IEmailSender emailSender;

		public CartController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
		{
            this.db = db;
			this.webHostEnvironment = webHostEnvironment;
			this.emailSender = emailSender;
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

		public IActionResult InquiryConfirmation()
		{
			HttpContext.Session.Clear(); // очистить данные сессии
			
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
		{
			// код для отправки сообщения
			// combine
			var path = webHostEnvironment.WebRootPath += Path.DirectorySeparatorChar.ToString() +
				"templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

			var subject = "New Order";

			var bodyHtml = "";

			using (StreamReader reader = new StreamReader(path))
			{
				bodyHtml = reader.ReadToEnd();
			}

			string textProducts = "";
			foreach (var item in productUserViewModel.ProductList)
			{
				textProducts += $"- Name: {item.Name}, ID: {item.Id}\n";
			}

			string body = string.Format(bodyHtml, 
				productUserViewModel.ApplicationUser.FullName,
				productUserViewModel.ApplicationUser.Email, 
				productUserViewModel.ApplicationUser.PhoneNumber, 
				textProducts);

			await emailSender.SendEmailAsync(productUserViewModel.ApplicationUser.Email, subject, body);
			await emailSender.SendEmailAsync("Max2827@yandex.ru", subject, body);

			return RedirectToAction("InquiryConfirmation");
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
				ProductList = productList.ToList()
			};

			return View(productUserViewModel);
		}

	}
}
