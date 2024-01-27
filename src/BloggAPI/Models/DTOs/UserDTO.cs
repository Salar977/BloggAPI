namespace BloggAPI.Models.DTOs;

public record UserDTO(int Id,
					  string? UserName,
					  string? FirstName,
					  string? LastName,
					  string? Email,
					  DateTime? Created,
					  DateTime? Updated);
