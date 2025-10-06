using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingDataModel.Entities
{
    /// <summary>
    /// Represents an RDP file that students and training agency can access
    /// </summary>
    [Table("rdp_files")]
    public class RdpFile
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("file_name")]
        public string FileName { get; set; } = string.Empty;

        [Column("file_path")]
        public string? FilePath { get; set; }

        [Column("virtual_machine_id")]
        public int VirtualMachineId { get; set; }

        [Column("student_id")]
        public int? StudentId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("VirtualMachineId")]
        public VirtualMachine VirtualMachine { get; set; } = null!;

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
    }
}
