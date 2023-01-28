using Microsoft.AspNetCore.Identity;

namespace TurboFishShop_Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
	}
}
