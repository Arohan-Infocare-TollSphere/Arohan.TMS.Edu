using Arohan.TMS.Application.Interfaces;
using Arohan.TMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Arohan.TMS.Web.Middleware
{
    /// <summary>
    /// Middleware that resolves the tenant via ITenantProvider and sets the ApplicationDbContext.CurrentTenantId for the request scope.
    /// Must run before any code that uses the DbContext (controllers, pages, etc.).
    /// </summary>
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantMiddleware> _logger;

        public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var tenantProvider = context.RequestServices.GetService<ITenantProvider>();
                var db = context.RequestServices.GetService<ApplicationDbContext>();

                if (tenantProvider != null && db != null)
                {
                    // If tenant provider returns null, CurrentTenantId remains null and tenant-filter will block queries.
                    db.CurrentTenantId = tenantProvider.TenantId;
                    _logger.LogDebug("TenantMiddleware set ApplicationDbContext.CurrentTenantId = {TenantId}", tenantProvider.TenantId);
                }
            }
            catch (Exception ex)
            {
                // Keep middleware resilient: don't block requests for tenant resolution failure here.
                _logger.LogWarning(ex, "TenantMiddleware failed to set tenant on DbContext.");
            }

            await _next(context);
        }
    }
}
