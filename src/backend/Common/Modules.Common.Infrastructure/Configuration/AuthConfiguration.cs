namespace Modules.Common.Infrastructure.Configuration;

public record AuthConfiguration
{
	public required string Key { get; init; }
	public required string Issuer { get; init; }
	public required string Audience { get; init; }
}
