using FluentValidation;
using Blogg.Models.AddDTO;

namespace Blogg.Validators;

public class CommentAddDTOValidator : AbstractValidator<CommentAddDTO>
{
    public CommentAddDTOValidator()
    {
        RuleFor(x => x.Kommentar)
            .NotEmpty().WithMessage("Kommentaren kan ikke være tom.")
            .MinimumLength(3).WithMessage("Kommentaren må være minst 3 tegn.")
            .MaximumLength(5000).WithMessage("Kommentaren kan ikke være på mer enn 5000 tegn.");
    }
}