using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextHome.Realty.Domain.Entities;

public class Booking
{
    [Key] public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser? User { get; set; }

    [Required]
    [ForeignKey(nameof(VillaId))]
    public int VillaId { get; set; }

    public Villa? Villa { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;

    [Required] public double TotalCost { get; set; }
    public int Nights { get; set; }
    public string? Status { get; set; } = string.Empty;

    [Required] public DateTime BookingDate { get; set; }
    [Required] public DateOnly CheckInDate { get; set; }
    [Required] public DateOnly CheckOutDate { get; set; }

    public bool IsPaymentSuccessful { get; set; } = false;
    public DateTime PaymentDate { get; set; }

    public string? StripeSessionId { get; set; }
    public string? StripePaymentIntentId { get; set; }

    public DateTime ActualCheckInDate { get; set; }
    public DateTime ActualCheckOutDate { get; set; }

    public int VillaNumber { get; set; }

    [NotMapped] public List<VillaNumber>?VillaNumbers { get; set; }
}