using Microsoft.AspNetCore.Identity;

namespace Modules.Users.Domain.Users;

public class UserRole : IdentityUserRole<string>
{
	public User User { get; set; } = null!;
	public Role Role { get; set; } = null!;
}
