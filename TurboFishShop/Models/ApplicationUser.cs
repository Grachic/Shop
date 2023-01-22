﻿using Microsoft.AspNetCore.Identity;

namespace TurboFishShop.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
	}
}
