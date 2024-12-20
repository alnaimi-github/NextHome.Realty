﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextHome.Realty.Domain.Entities;

public class VillaImage
{
    public int Id { get; set; }
    [Required] public string ImageUrl { get; set; } = string.Empty;
    public int VillaId { get; init; }
    [ForeignKey(nameof(VillaId))] public Villa? Villa { get; set; }
}