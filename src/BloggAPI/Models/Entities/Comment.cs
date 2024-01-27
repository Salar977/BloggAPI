using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggAPI.Models.Entities;

public class Comment
{
	[Key]
	public int Id { get; set; }

	[ForeignKey(nameof(PostId))]
	public int PostId { get; set; }

	[ForeignKey(nameof(UserId))]
	public int UserId { get; set; }

	[Required]
	public string? Content { get; set; }

	[Required]
	public DateTime Created { get; set; }

	[Required]
	public DateTime Updated { get; set; }

	// Navigation Properties
	public virtual Post? Post { get; set; }
	public virtual User? User {  get; set; }
}
