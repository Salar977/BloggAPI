using FluentValidation;
using Blogg.Models.AddDTO;

namespace Blogg.Validators;

public class PostAddDTOValidator : AbstractValidator<PostAddDTO>
{
    public PostAddDTOValidator()
    {
        RuleFor(x => x.Tittel)
            .NotEmpty().WithMessage("Tittel kan ikke være tom.")
            .MinimumLength(3).WithMessage("Tittel må være minst 3 tegn.")
            .MaximumLength(50).WithMessage("Tittel kan ikke ha mer enn 50 tegn.");

        RuleFor(x => x.Innlegg)
            .NotEmpty().WithMessage("Innlegg kan ikke være tom.")
            .MinimumLength(5).WithMessage("Innlegg må være minst 5 tegn.");

    }
}