using System;
using System.Linq;
using Ds3App2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ds3App2.Startup))]
namespace Ds3App2
{
    public partial class Startup
    {
        private readonly string email = "admin@repairandservice.co.za";
        private readonly string password = "Password01";
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AddRoles();
            AddAdmin();
        }

        private void AddRoles()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                if (!roleManger.RoleExists("Admin"))
                {
                    var role = new IdentityRole("Admin");
                    roleManger.Create(role);
                }
                if (!roleManger.RoleExists("Customer"))
                {
                    var role = new IdentityRole("Customer");
                    roleManger.Create(role);
                }
                if (!roleManger.RoleExists("Mechanic"))
                {
                    var role = new IdentityRole("Mechanic");
                    roleManger.Create(role);
                }
            }
        }
        private void AddAdmin()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if(!context.Users.Any(c => c.Email == email))
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var user = new ApplicationUser
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Contact = "08100000000",
                        UserName = email,
                        Email = email,
                    };

                    var newUser = userManager.Create(user, password);
                    if (newUser.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Admin");
                    }
                }
      
            }
        }
    }
}
