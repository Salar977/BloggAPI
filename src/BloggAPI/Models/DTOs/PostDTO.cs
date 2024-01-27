namespace BloggAPI.Models.DTOs;

public record PostDTO(int PostId,
                      int UserId,
                      string? Title,
                      string? Content,
                      DateTime? Created,
                      DateTime? Updated);