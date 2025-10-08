using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("VirtualBank")]
public partial class VirtualBank
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string BankName { get; set; } = null!;

    public int WalletId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    public TimeOnly? UpdatedAt { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [InverseProperty("Bank")]
    public virtual ICollection<UserBankAccount> UserBankAccounts { get; set; } = new List<UserBankAccount>();

    [ForeignKey("WalletId")]
    [InverseProperty("VirtualBanks")]
    public virtual Wallet Wallet { get; set; } = null!;
}
