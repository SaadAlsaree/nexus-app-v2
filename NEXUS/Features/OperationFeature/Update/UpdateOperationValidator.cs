using FluentValidation;

namespace NEXUS.Features.OperationFeature.Update;

public class UpdateOperationValidator : AbstractValidator<UpdateOperationCommand>
{
    public UpdateOperationValidator()
    {
        RuleFor(x => x.OperationId).NotEmpty();
        RuleFor(x => x.OperationType).IsInEnum();
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.RoleInOp).MaximumLength(100);
    }
}
