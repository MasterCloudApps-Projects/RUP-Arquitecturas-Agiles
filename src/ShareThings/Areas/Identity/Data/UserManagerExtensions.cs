using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShareThings.Data.Context;
using ShareThings.Domain;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareThings.Areas.Identity.Data
{
    public static class UserManagerExtensions
    {
        public static async Task<User> FindUserDomain(this UserManager<ShareThingsUser> userManager, IShareThingsDbContext context, ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            ShareThingsUser userIdentity = await userManager.GetUserAsync(user);
            User userDomain = await context.Users.SingleOrDefaultAsync(u => u.UserIdentityId.Equals(userIdentity.Id));
            return userDomain;
        }
    }
}