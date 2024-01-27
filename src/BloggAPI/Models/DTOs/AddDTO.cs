namespace BloggAPI.Models.AddDTO;

public record PostAddDTO(string Tittel,
                         string Innlegg);


public record CommentAddDTO(string Kommentar);


public record UserRegistrationDTO(string UserName,
                                  string FirstName,
                                  string LastName,
                                  string Password,
                                  string Email
    );