using HutechITEvent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HutechITEvent.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            // Ensure database is created
            await context.Database.MigrateAsync();

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Name = "H?i th?o Công ngh?", Description = "Các h?i th?o v? công ngh? thông tin", Type = CategoryType.Event },
                    new Category { Name = "Workshop", Description = "Các workshop k? n?ng", Type = CategoryType.Event },
                    new Category { Name = "Seminar", Description = "Các bu?i seminar chuyên ??", Type = CategoryType.Event },
                    new Category { Name = "Cu?c thi L?p trình", Description = "Các cu?c thi v? l?p trình", Type = CategoryType.Contest },
                    new Category { Name = "Hackathon", Description = "Cu?c thi phát tri?n s?n ph?m", Type = CategoryType.Contest },
                    new Category { Name = "Cu?c thi Ý t??ng", Description = "Cu?c thi v? ý t??ng kh?i nghi?p", Type = CategoryType.Contest }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed Admin User
            if (!await userManager.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    UserName = "admin@hutech.edu.vn",
                    Email = "admin@hutech.edu.vn",
                    FullName = "Qu?n tr? viên",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
                }

                // Seed Lecturer
                var lecturer = new User
                {
                    UserName = "lecturer@hutech.edu.vn",
                    Email = "lecturer@hutech.edu.vn",
                    FullName = "Nguy?n V?n A",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                result = await userManager.CreateAsync(lecturer, "Lecturer@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(lecturer, RoleNames.Lecturer);
                }

                // Seed Student
                var student = new User
                {
                    UserName = "student@hutech.edu.vn",
                    Email = "student@hutech.edu.vn",
                    FullName = "Tr?n V?n B",
                    PhoneNumber = "0123456789",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                result = await userManager.CreateAsync(student, "Student@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(student, RoleNames.Student);

                    // Create Student record
                    var studentRecord = new Student
                    {
                        StudentId = "2180607123",
                        UserId = student.Id,
                        FullName = student.FullName,
                        Email = student.Email,
                        PhoneNumber = student.PhoneNumber ?? "",
                        Class = "DHKTPM18A",
                        Faculty = "Công ngh? Thông tin",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    context.Students.Add(studentRecord);
                    await context.SaveChangesAsync();
                }
            }

            // Seed Sample Events
            if (!context.Events.Any())
            {
                var category = await context.Categories.FirstOrDefaultAsync(c => c.Type == CategoryType.Event);
                var organizer = await userManager.FindByEmailAsync("lecturer@hutech.edu.vn");

                if (category != null && organizer != null)
                {
                    var events = new[]
                    {
                        new Event
                        {
                            Title = "H?i th?o Trí tu? Nhân t?o 2024",
                            Description = "H?i th?o v? xu h??ng và ?ng d?ng AI trong th?c t?. Di?n gi? là các chuyên gia hàng ??u trong l?nh v?c AI.",
                            StartDate = DateTime.Now.AddDays(7),
                            EndDate = DateTime.Now.AddDays(7).AddHours(3),
                            Location = "H?i tr??ng A, Tòa nhà B, HUTECH",
                            MaxParticipants = 200,
                            CategoryId = category.Id,
                            OrganizerId = organizer.Id,
                            Status = EventStatus.Published,
                            CreatedAt = DateTime.Now,
                            ThumbnailUrl = "https://images.unsplash.com/photo-1591453089816-0fbb971b454c?w=800"
                        },
                        new Event
                        {
                            Title = "Workshop: L?p trình Web v?i ASP.NET Core",
                            Description = "Workshop th?c hành xây d?ng ?ng d?ng web v?i ASP.NET Core 8.0 và Entity Framework Core.",
                            StartDate = DateTime.Now.AddDays(14),
                            EndDate = DateTime.Now.AddDays(14).AddHours(4),
                            Location = "Phòng Lab 401, Tòa nhà C",
                            MaxParticipants = 50,
                            CategoryId = category.Id,
                            OrganizerId = organizer.Id,
                            Status = EventStatus.Published,
                            CreatedAt = DateTime.Now,
                            ThumbnailUrl = "https://images.unsplash.com/photo-1517694712202-14dd9538aa97?w=800"
                        },
                        new Event
                        {
                            Title = "Seminar: Cloud Computing và DevOps",
                            Description = "Tìm hi?u v? Cloud Computing, Docker, Kubernetes và CI/CD pipeline.",
                            StartDate = DateTime.Now.AddDays(21),
                            EndDate = DateTime.Now.AddDays(21).AddHours(2),
                            Location = "Phòng 305, Tòa nhà A",
                            MaxParticipants = 100,
                            CategoryId = category.Id,
                            OrganizerId = organizer.Id,
                            Status = EventStatus.Published,
                            CreatedAt = DateTime.Now,
                            ThumbnailUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=800"
                        }
                    };

                    context.Events.AddRange(events);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
