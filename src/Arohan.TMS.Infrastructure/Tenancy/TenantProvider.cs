using System.Security.Claims;
using Arohan.TMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Arohan.TMS.Infrastructure.Tenancy
{
    /// <summary>
    /// Resolves TenantId from:
    /// 1) JWT claim "tenantId" (string GUID) OR
    /// 2) HTTP header "X-Tenant-Id"
    /// Use this implementation in DI as the ITenantProvider.
    /// </summary>
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid? _cachedTenant;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? TenantId
        {
            get
            {
                if (_cachedTenant != null) return _cachedTenant;

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return null;

                // 1) Check claims (common when using JWT)
                var claim = httpContext.User?.FindFirst("tenantId") ?? httpContext.User?.FindFirst("tenant_id");
                if (claim != null && Guid.TryParse(claim.Value, out var tidFromClaim))
                {
                    _cachedTenant = tidFromClaim;
                    return _cachedTenant;
                }

                // 2) Check header X-Tenant-Id
                if (httpContext.Request.Headers.TryGetValue("X-Tenant-Id", out var headerVals))
                {
                    var header = headerVals.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(header) && Guid.TryParse(header, out var tidFromHeader))
                    {
                        _cachedTenant = tidFromHeader;
                        return _cachedTenant;
                    }
                }

                // Not resolved
                return null;
            }
        }

        public void EnsureTenant()
        {
            if (TenantId == null)
                throw new UnauthorizedAccessException("Tenant not resolved. Ensure X-Tenant-Id header or tenantId claim is present.");
        }
    }
}
