using FluentValidation;

namespace Modules.Users.Features.Users.UpdateUserRole;

public class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
{
    public UpdateUserRoleRequestValidator()
    {
        RuleFor(x => x.NewRole)
            .NotEmpty().WithMessage("New role is required");
    }
}
