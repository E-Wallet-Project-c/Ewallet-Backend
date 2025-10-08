using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("Admin")]
[Index("Phone", "Email", Name = "Uniqe-value", IsUnique = true)]
public partial class Admin
{
    [Key]
    public int Id { get; set; }

    [Column("Phone ")]
    [StringLength(50)]
    public string Phone { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column("FullName ")]
    [StringLength(50)]
    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    [StringLength(50)]
    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    [Column("IsActive ")]
    public bool IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CraetedAt { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Admins")]
    public virtual Role Role { get; set; } = null!;
}
