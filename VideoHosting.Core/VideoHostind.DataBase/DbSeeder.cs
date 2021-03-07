using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using VideoHosting.Domain.Entities;

namespace VideoHosting.DataBase
{
    public class DbSeeder
    {
        public async Task Seed(UserManager<User> userManager, RoleManager<UserRole> roleManager, DataBaseContext context)
        {
            UserRole role1 = new UserRole { Name = "Admin", Description = "Can delete video, users, commentaries" };
            UserRole role2 = new UserRole { Name = "User", Description = "Can add video, commentaries, likes" };
            UserRole role3 = new UserRole { Name = "TsarBatushka", Description = "Can do everything that admin and can add,delete them" };

            await roleManager.CreateAsync(role2);
            await roleManager.CreateAsync(role1);
            await roleManager.CreateAsync(role3);

            context.SaveChanges();


            User user1 = new User
            {
                Email = "dyshkant2804@ukr.net", 
                UserName = "AllaDyshkant", 
                PhoneNumber = "380683925657",
                Name = "Alla",
                Surname = "Dyshkant",
                DateOfCreation = DateTime.Now,
                TempPassword = 0,
                Sex = false
            };

            var r1 = await userManager.CreateAsync(user1, "Dyshkant2804");
            user1.UserName = user1.Id;
            var r11 = await userManager.UpdateAsync(user1);

            User user2 = new User { 
                Email = "dyshkant280400@ukr.net",
                UserName = "OlegDyshkant", 
                PhoneNumber = "380683925658",
                Name = "Oleg",
                Surname = "Dyshkant",
                DateOfCreation = DateTime.Now,
                TempPassword = 0,
                Sex = true
            };
            
            var r2 = await userManager.CreateAsync(user2, "Dyshkant280400");
            user2.UserName = user2.Id;
            var r22 = await userManager.UpdateAsync(user2);

            User admin = new User 
            { 
                Email = "vivavictro28@ukr.net", 
                UserName = "Tsar", 
                PhoneNumber = "380682819737",
                Name = "Victor",
                Surname = "Dyshkant",
                DateOfCreation = DateTime.Now,
                TempPassword = 0,
                Sex = true
            };
            
            var r3 = await userManager.CreateAsync(admin, "Qwerty280400");
            admin.UserName = admin.Id;
            var r33 =  userManager.UpdateAsync(admin);

         
            //context.Users.Add(User1);
            //context.Users.Add(User2);
            //context.Users.Add(adminProfile);


            await userManager.AddToRoleAsync(admin, role1.Name);
            await userManager.AddToRoleAsync(admin, role2.Name);
            await userManager.AddToRoleAsync(admin, role3.Name);

            await userManager.AddToRoleAsync(user1, role2.Name);
            await userManager.AddToRoleAsync(user2, role2.Name);

            await context.SaveChangesAsync();
        }
    }
}
