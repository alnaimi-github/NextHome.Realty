using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextHome.Realty.Domain.Entities;

public class Villa
{
    public int Id { get; set; }
    [MaxLength(50)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int Sqft { get; set; }

    [Range(1, 10)] public int Occupancy { get; set; }

    //[Display(Name ="Image Url")]
    //public string? ImageUrl { get; set; } = string.Empty;
    [Display(Name = "Price per night")]
    [Range(10, 10000)]
    public double Price { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public List<VillaImage>? VillaImages { get; set; }
    public IEnumerable<Amenity>?VillaAmenity { get; set; }

    [NotMapped] public bool IsAvailable { get; set; } = true;
}