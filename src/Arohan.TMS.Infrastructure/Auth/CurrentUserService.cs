using System.Security.Claims;
using Arohan.TMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Arohan.TMS.Infrastructure.Auth
{
    /// <summary>
    /// Reads user information from HttpContext.User claims.
    /// Standard claim names are checked in this order:
    /// - "sub" or ClaimTypes.NameIdentifier for UserId
    /// - "name", "preferred_username", ClaimTypes.Name for UserName
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? UserId
        {
            get
            {
                if (User == null) return null;
                var sub = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return string.IsNullOrWhiteSpace(sub) ? null : sub;
            }
        }

        public string? UserName
        {
            get
            {
                if (User == null) return null;
                var name = User.FindFirst("name")?.Value
                           ?? User.FindFirst("preferred_username")?.Value
                           ?? User.Identity?.Name;
                return string.IsNullOrWhiteSpace(name) ? null : name;
            }
        }

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
