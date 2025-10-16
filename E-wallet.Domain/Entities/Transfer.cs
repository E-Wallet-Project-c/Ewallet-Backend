using E_wallet.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_wallet.Domain.Entities;

[Table("Transfer")]
public partial class Transfer
{
    [Key]
    public int Id { get; set; }

    public int SenderWalletId { get; set; }

    public int ReciverWalletId { get; set; }

    public int Amount { get; set; }

    [Column(TypeName = "character varying")]
    public TransferStatus Status { get; set; } = TransferStatus.Pending;

    public double? Fee { get; set; }

    public bool IsActive { get; set; } = true;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [ForeignKey("ReciverWalletId")]
    [InverseProperty("TransferReciverWallets")]
    public virtual Wallet ReciverWallet { get; set; } = null!;

    [ForeignKey("SenderWalletId")]
    [InverseProperty("TransferSenderWallets")]
    public virtual Wallet SenderWallet { get; set; } = null!;

    [InverseProperty("Transfer")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
