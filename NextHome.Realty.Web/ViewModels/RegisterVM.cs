using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NextHome.Realty.Web.ViewModels;

public class RegisterVM : ShardProperitiesIdentityVM
{
    [Required] public string Name { get; set; } = string.Empty;

    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Phone Number")] public string? PhoneNumber { get; set; }

    public string? Role { get; set; }
    public IEnumerable<SelectListItem>?RoleListItems { get; set; }
}