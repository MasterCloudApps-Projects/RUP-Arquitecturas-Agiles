using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareThings.Areas.Identity.Data;

namespace ShareThings.Areas.Identity.Data
{
    public class ShareThingsIdentityContext : IdentityDbContext<ShareThingsUser>
    {
        public ShareThingsIdentityContext(DbContextOptions<ShareThingsIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
