using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("Notification")]
public partial class Notification
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    [Column("Event ")]
    [StringLength(50)]
    public string Event { get; set; } = null!;

    [StringLength(50)]
    public string Type { get; set; } = null!;

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "time with time zone")]
    public DateTimeOffset? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}
