using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("Beneficiary ")]
public partial class Beneficiary
{
    [Key]
    public int Id { get; set; }

    public int WalletId { get; set; }

    public int BeneficiaryWalletId { get; set; }

    [StringLength(50)]
    public string NickName { get; set; } = null!;

    [StringLength(50)]
    public string? Purpos { get; set; }

    public bool? IsActive { get; set; }

    public TimeOnly? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    public TimeOnly? UpdatedAt { get; set; }

    [Column(" UpdatedBy")]
    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [ForeignKey("BeneficiaryWalletId")]
    [InverseProperty("BeneficiaryBeneficiaryWallets")]
    public virtual Wallet BeneficiaryWallet { get; set; } = null!;

    [ForeignKey("WalletId")]
    [InverseProperty("BeneficiaryWallets")]
    public virtual Wallet Wallet { get; set; } = null!;
}
