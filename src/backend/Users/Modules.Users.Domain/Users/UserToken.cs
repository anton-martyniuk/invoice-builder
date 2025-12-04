using Microsoft.AspNetCore.Identity;

namespace Modules.Users.Domain.Users;

public class UserToken : IdentityUserToken<string>
{
	public User User { get; set; } = null!;
}
