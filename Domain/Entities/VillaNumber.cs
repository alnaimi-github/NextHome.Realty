using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextHome.Realty.Domain.Entities;

public class VillaNumber
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Villa Number")]
    public int Villa_Number { get; set; }

    [ForeignKey(nameof(VillaId))] public int VillaId { get; set; }
    public Villa? Villa { get; set; }
    public string? SpecialDetails { get; set; } = string.Empty;
}