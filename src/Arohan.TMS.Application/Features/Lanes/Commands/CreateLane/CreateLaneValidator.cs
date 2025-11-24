using FluentValidation;

namespace Arohan.TMS.Application.Features.Lanes.Commands.CreateLane
{
    public class CreateLaneValidator : AbstractValidator<CreateLaneCommand>
    {
        public CreateLaneValidator()
        {
            RuleFor(x => x.PlazaId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DocumentUrl).MaximumLength(1000).When(x => x.DocumentUrl != null);
            RuleFor(x => x.CredentialsRef).MaximumLength(500).When(x => x.CredentialsRef != null);
            RuleFor(x => x.AuthType).NotNull();
        }
    }
}
