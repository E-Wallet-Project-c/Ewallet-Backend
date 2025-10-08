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

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(50)]
    public string? Country { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [InverseProperty("Profile")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
