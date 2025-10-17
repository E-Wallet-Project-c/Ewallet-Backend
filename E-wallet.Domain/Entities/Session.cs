using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("Session")]
public partial class Session
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "character varying")]
    public string? RefreshToken { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime? UpdatedAt { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "timestamp with time zone")]

    public DateTime? ExpiresAt { get; set; }

    [Column(TypeName = "timestamp with time zone")]

    public DateTime? RefreshExpiresAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Sessions")]
    public virtual User User { get; set; } = null!;
}
