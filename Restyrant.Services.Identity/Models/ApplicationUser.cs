using Microsoft.AspNetCore.Identity;

namespace Restyrant.Services.Identity.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FisrtName { get; set; }
        public string LastName { get; set; }
    }
}
