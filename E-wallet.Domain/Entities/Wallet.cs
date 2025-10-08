using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("Wallet")]
public partial class Wallet
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [StringLength(50)]
    public string? Currency { get; set; }

    public double? Ballance { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActive { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("BeneficiaryWallet")]
    public virtual ICollection<Beneficiary> BeneficiaryBeneficiaryWallets { get; set; } = new List<Beneficiary>();

    [InverseProperty("Wallet")]
    public virtual ICollection<Beneficiary> BeneficiaryWallets { get; set; } = new List<Beneficiary>();

    [InverseProperty("Wallet")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [InverseProperty("ReciverWallet")]
    public virtual ICollection<Transfer> TransferReciverWallets { get; set; } = new List<Transfer>();

    [InverseProperty("SenderWallet")]
    public virtual ICollection<Transfer> TransferSenderWallets { get; set; } = new List<Transfer>();

    [ForeignKey("UserId")]
    [InverseProperty("Wallets")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Wallet")]
    public virtual ICollection<VirtualBank> VirtualBanks { get; set; } = new List<VirtualBank>();
}
