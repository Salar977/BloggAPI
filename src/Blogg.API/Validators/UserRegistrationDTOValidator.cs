using FluentValidation;
using Blogg.Data;
using Blogg.Models.AddDTO;

namespace Blogg.Validators;

public class UserRegistrationDTOValidator : AbstractValidator<UserRegistrationDTO>
{
    public UserRegistrationDTOValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username må være med.")
            .MinimumLength(3).WithMessage("Username må ha minst 3 tegn!")
            .MaximumLength(16).WithMessage("Username kan ikke være mer enn 16 tegn.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Firstname må være med.")
            .MinimumLength(3).WithMessage("UserName må ha minst 3 tegn.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName må være med.")
            .MinimumLength(3).WithMessage("UserName må ha minst 3 tegn.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password må være med.")
            .MinimumLength(3).WithMessage("UserName må ha minst 3 tegn.")
            .MaximumLength(16).WithMessage("Passord kan ikke være lenge enn 16 tegn.")
            //.Matches(@"[A-Z]+").WithMessage("Du må minst ha med en stor bokstav i passordet.")
            .Matches(@"[a-z]+").WithMessage("Du må minst ha med en liten bokstav i passordet.")
            .Matches(@"[0-9]+").WithMessage("Du må minst ha med et tall i passordet.");
        //  .Matches(@"[!?*#_]+").WithMessage("Passordet må innholde en av disse tegnene (! ? * # _)");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email må være med.")
            .EmailAddress().WithMessage("Må ha en gyldig email!");
    }
}
