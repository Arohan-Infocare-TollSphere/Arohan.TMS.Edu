using System;
using System.Collections.Generic;
using System.Text;


namespace Arohan.TMS.Application.Interfaces
{
    public interface ITenantProvider
    {
        /// <summary>Current tenant id (null if tenant-less or not resolved)</summary>
        Guid? TenantId { get; }

        /// <summary>Throws if tenant not set or unauthorized</summary>
        void EnsureTenant();
    }
}
