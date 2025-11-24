namespace Arohan.TMS.Application.Interfaces
{
    /// <summary>
    /// Abstraction to obtain the current authenticated user's identity (claims).
    /// Lightweight: avoids coupling handlers to HttpContext directly.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>Returns the current user's unique id (subject) if available.</summary>
        string? UserId { get; }

        /// <summary>Returns the current user's username or display name if available.</summary>
        string? UserName { get; }

        /// <summary>True if the user is authenticated.</summary>
        bool IsAuthenticated { get; }
    }
}
