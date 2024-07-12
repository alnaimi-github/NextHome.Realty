using Microsoft.AspNetCore.Identity;

namespace NextHome.Realty.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}