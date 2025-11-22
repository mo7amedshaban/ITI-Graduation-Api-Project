using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.Identity;


public partial class RefreshToken : AuditableEntity
{
    #region Properties
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }

    public string SessionId { get; set; } = string.Empty;
    public bool IsRevoked { get; set; }

    public ApplicationUser User { get; set; } = null!;

    #endregion

    #region Constructors
    public RefreshToken() { } 

    public RefreshToken(string token, Guid userId, DateTime expiresOnUtc, string sessionId)
    {
        Token = token;
        UserId = userId;
        ExpiresOnUtc = expiresOnUtc;
        SessionId = sessionId;
        IsRevoked = false;
    }
    #endregion

    #region  Behaviors

   
    

    #endregion


}
