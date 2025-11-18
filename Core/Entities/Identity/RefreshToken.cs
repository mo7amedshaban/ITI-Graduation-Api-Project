using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.Identity;


public partial class RefreshToken : AuditableEntity
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }

    public string SessionId { get; set; } = string.Empty;
    public bool IsRevoked { get; set; }

    public ApplicationUser User { get; set; } = null!;

}
