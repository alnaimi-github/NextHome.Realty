using System.ComponentModel.DataAnnotations;

namespace NextHome.Realty.Web.ViewModels;

public abstract class ShardProperitiesIdentityVM
{
    [Required] public string Email { get; set; } = string.Empty;


    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string? RedirectUrl { get; set; }
}