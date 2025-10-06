using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingDataModel.Entities
{
    /// <summary>
    /// Represents VM configuration options (SKU, Offer, Version, ISO VHD)
    /// </summary>
    [Table("vm_options")]
    public class VmOption
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column("sku")]
        public string? Sku { get; set; }

        [MaxLength(100)]
        [Column("offer")]
        public string? Offer { get; set; }

        [MaxLength(100)]
        [Column("version")]
        public string? Version { get; set; }

        [MaxLength(500)]
        [Column("iso_vhd")]
        public string? IsoVhd { get; set; }

        [Column("vm_type_id")]
        public int VmTypeId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("VmTypeId")]
        public VmType VmType { get; set; } = null!;
    }
}
