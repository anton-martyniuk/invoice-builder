using Microsoft.AspNetCore.Identity;

namespace Modules.Users.Domain.Users;

public class Role : IdentityRole
{
	public ICollection<UserRole> UserRoles { get; set; } = null!;
	public ICollection<RoleClaim> RoleClaims { get; set; } = null!;
}
