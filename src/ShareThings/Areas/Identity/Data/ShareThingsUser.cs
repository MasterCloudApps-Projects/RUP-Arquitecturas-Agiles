using Microsoft.AspNetCore.Identity;
using System;

namespace ShareThings.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ShareThingsUser class
    public class ShareThingsUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
