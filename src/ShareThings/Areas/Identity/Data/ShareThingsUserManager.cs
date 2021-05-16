using Microsoft.AspNetCore.Identity;
using ShareThings.Data.Context;
using ShareThings.Domain;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareThings.Areas.Identity.Data
{
    public class ShareThingsUserManager : IShareThingsUserManager
    {
        #region Attributes
        private readonly IShareThingsDbContext _context;
        private readonly UserManager<ShareThingsUser> _userManager;
        #endregion

        #region Constructor
        public ShareThingsUserManager(
            IShareThingsDbContext context,
            UserManager<ShareThingsUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }
        #endregion

        #region Methods
        public Task<User> FindUserDomain(ClaimsPrincipal user)
        {
            return this._userManager.FindUserDomain(this._context, user);
        }

        public string FindUserName(string userId)
        {
            return this._userManager.FindByIdAsync(userId).Result.UserName.ToString();
        }

        #endregion
    }
}
