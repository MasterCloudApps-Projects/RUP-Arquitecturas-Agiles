using ShareThings.Domain;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareThings.Areas.Identity.Data
{
    public interface IShareThingsUserManager
    {
        Task<User> FindUserDomain(ClaimsPrincipal user);

        string FindUserName(string userId);
    }
}
