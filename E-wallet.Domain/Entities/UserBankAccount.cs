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

    public int UserId { get; set; }

    public int BankId { get; set; }

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

    [ForeignKey("BankId")]
    [InverseProperty("UserBankAccounts")]
    public virtual VirtualBank Bank { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserBankAccounts")]
    public virtual User User { get; set; } = null!;
}
