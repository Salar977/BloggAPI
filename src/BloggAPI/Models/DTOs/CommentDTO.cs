namespace BloggAPI.Models.DTOs;

public record CommentDTO(int Id,
                        int PostId,
                        int UserId,
                        string? CommentContent,
                        DateTime? DateCommented,
                        DateTime? DateUpdated);