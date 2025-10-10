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

    [StringLength(50)]
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "character varying")]
    public string? Createdby { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column(TypeName = "character varying")]
    public string? Updatedby { get; set; }

    public int? ProfileId { get; set; } =null!;

    [Column(TypeName = "character varying")]
    public string FullName { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [ForeignKey("ProfileId")]
    [InverseProperty("Users")]
    public virtual Profile Profile { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    [InverseProperty("User")]
    public virtual ICollection<UserBankAccount> UserBankAccounts { get; set; } = new List<UserBankAccount>();

    [InverseProperty("User")]
    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
