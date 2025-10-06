using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingDataModel.Entities
{
    /// <summary>
    /// Represents a virtual machine used for training courses
    /// </summary>
    [Table("virtual_machines")]
    public class VirtualMachine
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column("ip_address")]
        public string? IpAddress { get; set; }

        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "Stopped";

        [Column("training_course_id")]
        public int? TrainingCourseId { get; set; }

        [Column("vm_type_id")]
        public int VmTypeId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("TrainingCourseId")]
        public TrainingCourse? TrainingCourse { get; set; }

        [ForeignKey("VmTypeId")]
        public VmType VmType { get; set; } = null!;

        public ICollection<DailyUsageStatistic> DailyUsageStatistics { get; set; } = new List<DailyUsageStatistic>();
        public ICollection<RdpFile> RdpFiles { get; set; } = new List<RdpFile>();
    }
}
