using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogg.Models.Entities
{
	public class Post
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(UserId))]
		public int UserId { get; set; }

		[Required]
		[MaxLength(200)]
		public string? Title { get; set; }

		[Required]
		public string? Content { get; set; }

		[Required]
		public DateTime Created { get; set; }

		[Required]
		public DateTime Updated { get; set; }

		// Navigation Properties
		public virtual User? User { get; set; }
		public virtual ICollection<Comment> Comment { get; set; } = new HashSet<Comment>();
	}
}
