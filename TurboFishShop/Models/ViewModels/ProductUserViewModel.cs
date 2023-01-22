namespace TurboFishShop.Models.ViewModels
{
	public class ProductUserViewModel
	{
		public ApplicationUser ApplicationUser { get; set; }
		public IList<Product> ProductList { get; set; }

		public ProductUserViewModel()
		{
			ProductList = new List<Product>();
		}
	}
}
