using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextHome.Realty.Domain.Entities;

public class Amenity
{
    [Key]public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [ForeignKey(nameof(VillaId))] public int VillaId { get; set; }
    public Villa? Villa { get; set; }
}