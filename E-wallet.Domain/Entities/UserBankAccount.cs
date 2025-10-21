using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("UserBankAccount")]
public partial class UserBankAccount
{
    [Key]
    public int Id { get; set; }
    public int WalletId { get; set; }

    [StringLength(100)]
    public string BankName { get; set; } = null!;  

    [Column(TypeName = "character varying")]
    public string AccountNumber { get; set; } = null!;

    public bool? IsActive { get; set; }

    public double Balance { get; set; }

    [Column(TypeName = "character varying")]
    public string? CreatedBy { get; set; }

    public TimeOnly? CreatedAt { get; set; }

    public TimeOnly? UpdatedAt { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }


    [ForeignKey("WalletId")]
    [InverseProperty("UserBankAccounts")]
    public virtual Wallet Wallet { get; set; } = null!;
}
