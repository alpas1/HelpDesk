using System;
using HelpDesk.Data;
using HelpDesk.Data.Static;
using HelpDesk.Models;
using Microsoft.AspNetCore.Identity;

namespace Lab10.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Services.Any())
                {
                    context.Services.AddRange(new List<Service>()
                    {
                        new Service()
                        {
                            name = "Networking",
                            description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Convallis convallis tellus id interdum velit laoreet id donec. Cursus euismod quis viverra nibh cras pulvinar mattis nunc. Curabitur vitae nunc sed velit.",
                            imageURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/106257638.jpg?k=4dd13a3f0f19081704cd50ee377462f2d13ea0662e577576e98bedc711005334&o=&hp=1"

                        },

                        new Service()
                        {
                            name = "CyberSecurity",
                            description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Convallis convallis tellus id interdum velit laoreet id donec. Cursus euismod quis viverra nibh cras pulvinar mattis nunc. Curabitur vitae nunc sed velit.",
                            imageURL = "https://media.millenniumhotels.com/Live/B/2/8/B286539D-B2DC-45C2-AA6C-C357236F61FF/Studio%20Suite.jpg?r=220722034325"

                        },


                    }
                    );

                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersandRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.Employee))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));


                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();


                string adminEmail = "admin@fixit.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAdminUser, "@Dmin123");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string employee1Email = "employee1@fixit.com";
                var employee1 = await userManager.FindByEmailAsync(employee1Email);
                if (employee1 == null)
                {
                    var newEmployeeUser = new User()
                    {
                        FirstName = "Employee",
                        LastName = "One",
                        UserName = "employee1",
                        Email = employee1Email,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newEmployeeUser, "@Dmin123");
                    await userManager.AddToRoleAsync(newEmployeeUser, UserRoles.Employee);
                }

                string user1Email = "user1@fixit.com";
                var user1 = await userManager.FindByEmailAsync(user1Email);
                if (user1 == null)
                {
                    var newUser = new User()
                    {
                        FirstName = "User",
                        LastName = "One",
                        UserName = "user1",
                        Email = user1Email,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newUser, "@Dmin123");
                    await userManager.AddToRoleAsync(newUser, UserRoles.User);
                }

                string user2Email = "user2@fixit.com";
                var user2 = await userManager.FindByEmailAsync(user2Email);
                if (user2 == null)
                {
                    var newUser = new User()
                    {
                        FirstName = "User",
                        LastName = "Two",
                        UserName = "user2",
                        Email = user2Email,
                        EmailConfirmed = true,
                    };
                    _ = await userManager.CreateAsync(newUser, "@Dmin123");
                    _ = await userManager.AddToRoleAsync(newUser, UserRoles.User);
                }
            }
        }
    }
}

