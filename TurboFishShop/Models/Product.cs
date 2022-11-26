using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurboFishShop.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Value must be more than 1.")]
		public double Price { get; set; }

		public string Image { get; set; }

		// явное добавление представления для внешнего ключа
		[Display(Name = "Category Id")]
		public int CategoryId { get; set; }

		// добавление внешнего ключа
		[ForeignKey("CategoryId")]
		public virtual Category Category { get; set; }
	}
}
