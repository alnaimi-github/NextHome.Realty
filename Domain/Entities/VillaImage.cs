using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHome.Realty.Domain.Entities
{
    public class VillaImage
    {
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public int VillaId { get; set; }
        [ForeignKey(nameof(VillaId))]
        public Villa? Villa { get; set; }
    }
}
