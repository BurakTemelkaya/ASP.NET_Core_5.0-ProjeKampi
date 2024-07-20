using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace CoreLayer.Extensions;

public class UserHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        throw new Exception("User ID claim is not found or invalid.");
    }

    public string GetUserName()
    {
        var userNameClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name);
        return userNameClaim?.Value ?? throw new Exception("User name claim is not found.");
    }

    public string GetUserEmail()
    {
        var userEmailClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email);
        return userEmailClaim?.Value ?? throw new Exception("User email claim is not found.");
    }
}
