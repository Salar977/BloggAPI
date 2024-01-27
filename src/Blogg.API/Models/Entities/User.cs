using System.ComponentModel.DataAnnotations;

namespace Blogg.Models.Entities;

public class User
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MinLength(3), MaxLength(50)]
	public string? UserName { get; set; }

	[Required]
    [MinLength(3), MaxLength(50)]
    public string? FirstName { get; set; }

	[Required]
    [MinLength(3), MaxLength(50)]
    public string? LastName { get; set; }

	[EmailAddress]
	public string? Email { get; set; }

	[Required]
	public DateTime? Created { get; set; }

	[Required]
	public DateTime? Updated { get; set; }

    public string? HashedPassword { get; set; }
	public string? Salt { get; set; }

	[Required]
	public bool IsAdminUser { get; set; }

	// Navigation Properties
	public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
	public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

}
