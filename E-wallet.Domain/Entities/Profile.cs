using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Entities;

[Table("Profile")]
[Index("Phone", Name = "Unique-Values", IsUnique = true)]
public partial class Profile 
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    [StringLength(50)]
    public string? Country { get; set; }


    [StringLength(50)]
    public string? Phone { get; set; }

    public bool SMSNotifications { get; set; }

    public bool EmailNotifications { get; set; }
    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? CreatedBy { get; set; }

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "time with time zone")]
    public DateTimeOffset? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }
    public User User { get; set; } = null!;


}
