using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("AuditLog")]
public partial class AuditLog
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "character varying")]
    public string ItemType { get; set; } = null!;

    [Column("OldItemSnapshotJSON")]
    public string OldItemSnapshotJson { get; set; } = null!;

    [Column("NewItemSnapshotJSON")]
    public string NewItemSnapshotJson { get; set; } = null!;

    public bool? IsActive { get; set; }

    public TimeOnly? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    public TimeOnly? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AuditLogs")]
    public virtual User User { get; set; } = null!;
}
