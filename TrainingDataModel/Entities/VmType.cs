using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingDataModel.Entities
{
    /// <summary>
    /// Represents a virtual machine type (e.g., Windows, Linux)
    /// </summary>
    [Table("vm_types")]
    public class VmType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<VmOption> VmOptions { get; set; } = new List<VmOption>();
        public ICollection<VirtualMachine> VirtualMachines { get; set; } = new List<VirtualMachine>();
    }
}
