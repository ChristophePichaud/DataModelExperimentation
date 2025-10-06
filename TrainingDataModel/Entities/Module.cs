using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingDataModel.Entities
{
    /// <summary>
    /// Represents a module within a training course
    /// </summary>
    [Table("modules")]
    public class Module
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

        [Column("order_number")]
        public int OrderNumber { get; set; }

        [Column("duration_hours")]
        public int? DurationHours { get; set; }

        [Column("training_course_id")]
        public int TrainingCourseId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("TrainingCourseId")]
        public TrainingCourse TrainingCourse { get; set; } = null!;
    }
}
