using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Restyrant.Services.Identity.DbContexts;
using Restyrant.Services.Identity.Models;

namespace Restyrant.Services.Identity.Intilizer
{
    public class DbIntilizer: IDbIntilizer
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbIntilizer(AppDbContext db,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Intilizer()
        {
            if (_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
            }

            if (_roleManager.FindByNameAsync(SD.Customer).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else
            {
                return; 
            }

            ApplicationUser adminUser = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed =true,
                PhoneNumber = "01284183702",
                FisrtName = "Mohamed",
                LastName = "Zonkol"
            };
            var result = _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
            
                _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();
                var temp = _userManager.AddClaimsAsync(adminUser, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, adminUser.FisrtName + " " + adminUser.LastName),
                    new Claim(JwtClaimTypes.GivenName, adminUser.FisrtName),
                    new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                    new Claim(JwtClaimTypes.Role, SD.Admin),
                }).Result;
            

            ApplicationUser customerUser = new ApplicationUser
            {
                UserName = "customer@gmail.com",
                Email = "customer@gmail.com",
                EmailConfirmed =true,
                PhoneNumber = "01205238482",
                FisrtName = "Karem",
                LastName = "Elsayed"
            };
            _userManager.CreateAsync(customerUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();
           var temp2= _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,customerUser.FisrtName+" "+customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName,customerUser.FisrtName),
                new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Customer),
            }).Result;

        }
    }
}
