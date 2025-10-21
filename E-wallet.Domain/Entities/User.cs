using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("User")]
[Index("Password", "Email", Name = "Uniqe-Values", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;
    public Profile? Profile { get; set; }
    public bool IsActive { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "character varying")]
    public string? Createdby { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }
    public string? OtpCode { get; set; } 
    public DateTime? OtpExpiry { get; set; }
    public bool IsVerified { get; set; }= false;

    [Column(TypeName = "character varying")]
    public string? Updatedby { get; set; }


    [Column(TypeName = "character varying")]
    public string FullName { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();



    [InverseProperty("User")]
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

   
    [InverseProperty("User")]
    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
