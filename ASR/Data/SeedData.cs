using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using ASR.Data;
using ASR.Models;

namespace Asr.Data
{
    public static class SeedData
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            using (var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>())
            using (var context = new AsrContext(serviceProvider.GetRequiredService<DbContextOptions<AsrContext>>()))
            {
                InitialiseUsers(userManager);
                InitialiseAsrData(context, userManager);
            }
        }

        private static void InitialiseUsers(UserManager<AppUser> userManager)
        {
            // Look for any users.

            if (userManager.Users.Any())
                return; // DB has been seeded.

            CreateUser(userManager, "e12345@rmit.edu.au");
            CreateUser(userManager, "e56789@rmit.edu.au");
            CreateUser(userManager, "s1234567@student.rmit.edu.au");
            CreateUser(userManager, "s4567890@student.rmit.edu.au");
        }

        private static void CreateUser(UserManager<AppUser> userManager, string userName) =>
            userManager.CreateAsync(new AppUser { UserName = userName, Email = userName }, "abc123").Wait();

        private static void InitialiseAsrData(AsrContext context, UserManager<AppUser> userManager)
        {
            // Look for any rooms.
            if (context.Room.Any())
                return; // DB has been seeded.

            context.Room.AddRange(
                new Room { RoomID = "A" },
                new Room { RoomID = "B" },
                new Room { RoomID = "C" },
                new Room { RoomID = "D" }
            );

            CreateStaff(context, "e12345", "Matt");
            CreateStaff(context, "e56789", "Matt");

            CreateStudent(context, "s1234567", "Kevin");
            CreateStudent(context, "s4567890", "Olivier");
                        
            context.Slot.AddRange(
                new Slot
                {
                    RoomID = "A",
                    StartTime = new DateTime(2019, 1, 30),
                    StaffID = "e12345"
                },
                new Slot
                {
                    RoomID = "B",
                    StartTime = new DateTime(2019, 1, 30),
                    StaffID = "e56789",
                    StudentID = "s1234567"
                }
            );            

            context.SaveChanges();

            UpdateUser(userManager, "e12345@rmit.edu.au", "e12345");
            UpdateUser(userManager, "e56789@rmit.edu.au", "e56789");
            UpdateUser(userManager, "s1234567@student.rmit.edu.au", "s1234567");
            UpdateUser(userManager, "s4567890@student.rmit.edu.au", "s4567890");
        }

        private static void CreateStaff(AsrContext context, string id, string name)
        {
            context.Staff.Add(new Staff
            {
                StaffID = id,
                Email = id + "@rmit.edu.au",
                Name = name
            });
        }

        private static void CreateStudent(AsrContext context, string id, string name)
        {
            context.Student.Add(new Student
            {
                StudentID = id,
                Email = id + "@student.rmit.edu.au",
                Name = name
            });
        }

        private static void UpdateUser(UserManager<AppUser> userManager, string userName, string id)
        {
            var user = userManager.FindByNameAsync(userName).Result;
            if (user.UserName.StartsWith('e'))
                user.StaffID = id;
            if (user.UserName.StartsWith('s'))
                user.StudentID = id;

            userManager.UpdateAsync(user).Wait();
        }
    }
}
