using IdentityModel;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mango.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            //if role does not exist, then running for first time
            if (_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                //need to wait for execution to complete
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else { return; }

            //user profile
            ApplicationUser adminUser = new ApplicationUser()
            {
                //some default prescribed properties in idenity server
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "3031110000",
                FirstName = "Joe",
                LastName = "Blow"
            };

            //create (first) user with password
            _userManager.CreateAsync(adminUser, "Password1!").GetAwaiter().GetResult();
            //assign user to admin
            _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();
            var user1 = _userManager.AddClaimsAsync(adminUser,
                new Claim[]
                   {
                        new Claim(JwtClaimTypes.Name, adminUser.FirstName+" "+adminUser.LastName),
                        new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                        new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                        new Claim(JwtClaimTypes.Role, SD.Admin)
                    }).Result;


            ApplicationUser customerUser = new ApplicationUser()
            {
                //some default prescribed properties in idenity server
                UserName = "customer@gmail.com",
                Email = "customer@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "3031110000",
                FirstName = "Jill",
                LastName = "Hill"
            };

            //create (first) user with password
            _userManager.CreateAsync(customerUser, "Password1!").GetAwaiter().GetResult();
            //add user to admin
            _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();
            var user2 = _userManager.AddClaimsAsync(customerUser,
                new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, customerUser.FirstName+" "+customerUser.LastName),
                        new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                        new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                        new Claim(JwtClaimTypes.Role, SD.Customer)
                    }).Result;



            _db.SaveChangesAsync();
        }
    }
}
