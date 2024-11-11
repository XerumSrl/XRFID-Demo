using System.Security.Claims;
using Xerum.XFramework.DBAccess;

namespace XRFID.Server.Demo.V2.Database;

public class XUserService(IHttpContextAccessor _httpContextAccessor) : IUserService
{
    public string GetCurrentUserId(string? user = null)
    {
        if (user == null || user == "system")
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
        }
        else
        {
            return user;
        }
    }
}
