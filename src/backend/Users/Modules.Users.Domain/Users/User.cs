using Microsoft.AspNetCore.Identity;
using Modules.Common.Domain;

namespace Modules.Users.Domain.Users;

public class User : IdentityUser, IAuditableEntity
{
	public ICollection<UserClaim> Claims { get; set; } = null!;

	public ICollection<UserRole> UserRoles { get; set; } = null!;

	public ICollection<UserLogin> UserLogins { get; set; } = null!;

	public ICollection<UserToken> UserTokens { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
