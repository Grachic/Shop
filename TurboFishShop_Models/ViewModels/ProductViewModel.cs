using Microsoft.AspNetCore.Mvc.Rendering;
using TurboFishShop_Models;

namespace TurboFishShop.Models.ViewModels
{
	public class ProductViewModel
	{
		public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoriesList { get; set; }
		public IEnumerable<SelectListItem> MyModelsList { get; set; }
	}
}
