using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_wallet.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("Limit")]
public partial class Limit
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public LimitScope Scope { get; set; } 

    [Column(TypeName = "character varying")]
    public LimitType Type { get; set; } 

    [Column("Value ")]
    public double Value { get; set; }

    public bool? IsActive { get; set; }

    public TimeOnly? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    public TimeOnly? UpdatedAt { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }
}
