using E_wallet.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_wallet.Domain.Entities;

[Table("Transaction")]
public partial class Transaction
{
    [Key]
    public int Id { get; set; }

    public int WalletId { get; set; }

    public double Amount { get; set; }

    [Column(TypeName = "character varying")]
    public TransactionType Type { get; set; }

    [Column(TypeName = "character varying")]
    public TransactionAction Action { get; set; } = TransactionAction.TopUp;

    public int BeneficiaryId { get; set; }

    public int? TransferId { get; set; }

    public bool? IsDeleted { get; set; }
    public bool? IsActive { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "character varying")]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("TransferId")]
    [InverseProperty("Transactions")]
    public virtual Transfer? Transfer { get; set; }

    [ForeignKey("WalletId")]
    [InverseProperty("Transactions")]
    public virtual Wallet Wallet { get; set; } = null!;
}
