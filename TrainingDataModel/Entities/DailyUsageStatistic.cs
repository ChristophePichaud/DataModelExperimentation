using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingDataModel.Entities
{
    /// <summary>
    /// Represents daily usage statistics for virtual machines
    /// </summary>
    [Table("daily_usage_statistics")]
    public class DailyUsageStatistic
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("virtual_machine_id")]
        public int VirtualMachineId { get; set; }

        [Column("usage_date")]
        public DateTime UsageDate { get; set; }

        [Column("hours_used")]
        public decimal HoursUsed { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("VirtualMachineId")]
        public VirtualMachine VirtualMachine { get; set; } = null!;
    }
}
