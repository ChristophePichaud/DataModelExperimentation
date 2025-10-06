using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingDataModel.Entities
{
    /// <summary>
    /// Represents a training course offering
    /// </summary>
    [Table("training_courses")]
    public class TrainingCourse
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("duration_hours")]
        public int? DurationHours { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("requires_vm")]
        public bool RequiresVm { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<VirtualMachine> VirtualMachines { get; set; } = new List<VirtualMachine>();
    }
}
