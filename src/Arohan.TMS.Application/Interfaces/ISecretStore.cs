using System;
using System.Collections.Generic;
using System.Text;

namespace Arohan.TMS.Application.Interfaces
{
    /// <summary>
    /// Resolves secret values from a secure vault at runtime.
    /// Implementations must not log secret plaintexts.
    /// </summary>
    public interface ISecretStore
    {
        /// <summary>Get a secret value by its vault path/key (e.g. "kv://tms/stripe/key").</summary>
        Task<string?> GetSecretAsync(string secretRef, CancellationToken ct = default);
    }
}
