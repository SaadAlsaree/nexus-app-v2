using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.EntityWatchlistFeature.Create;

public class CreateWatchlistEntryValidator : AbstractValidator<CreateWatchlistEntryCommand>
{
    public CreateWatchlistEntryValidator(AppDbContext db)
    {
        RuleFor(x => x.Keyword).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AlertLevel).IsInEnum();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);

        RuleFor(x => x.Keyword)
            .MustAsync(async (kw, ct) => !await db.EntityWatchlists.AnyAsync(w => w.Keyword == kw && !w.IsDeleted, ct))
            .WithMessage("This keyword is already on the watchlist.");
    }
}
