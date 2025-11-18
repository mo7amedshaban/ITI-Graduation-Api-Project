using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.Identity;

[Index("UserId", Name = "IX_RefreshTokens_UserId")]
public partial class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime ExpiresOnUtc { get; set; }

    public string SessionId { get; set; } = null!;

    public bool IsRevoked { get; set; }

    public DateTimeOffset? CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RefreshTokens")]
    public virtual ApplicationUser User { get; set; } = null!;
}
