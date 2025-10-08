using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingDataModel.Entities
{
    /// <summary>
    /// Represents an admin user for customers
    /// </summary>
    [Table("admin_users")]
    public class AdminUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }


    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

        [MaxLength(100)]
        [Column("username")]
        public string? Username { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }


        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
    }
}
