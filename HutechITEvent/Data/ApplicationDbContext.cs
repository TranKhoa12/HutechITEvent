using HutechITEvent.Models;
using Microsoft.EntityFrameworkCore;

namespace HutechITEvent.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventImage> EventImages { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<EventSchedule> EventSchedules { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<ContestImage> ContestImages { get; set; }
        public DbSet<ContestRegistration> ContestRegistrations { get; set; }
        public DbSet<ContestMember> ContestMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            });

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasIndex(r => r.Name).IsUnique();
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
                
                // Seed default roles
                entity.HasData(
                    new Role { Id = 1, Name = RoleNames.Admin, Description = "Qu?n tr? viên h? th?ng" },
                    new Role { Id = 2, Name = RoleNames.Lecturer, Description = "Gi?ng viên" },
                    new Role { Id = 3, Name = RoleNames.Student, Description = "Sinh viên" }
                );
            });

            // UserRole Configuration
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => ur.Id);
                
                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
            });

            // Student Configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.StudentId).IsUnique();
                entity.HasIndex(s => s.Email).IsUnique();
                entity.Property(s => s.StudentId).IsRequired().HasMaxLength(20);
                entity.Property(s => s.FullName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Email).IsRequired().HasMaxLength(100);
                
                entity.HasOne(s => s.User)
                      .WithOne(u => u.Student)
                      .HasForeignKey<Student>(s => s.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Category Configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            // Event Configuration
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(500);
                
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Events)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Organizer)
                      .WithMany(u => u.OrganizedEvents)
                      .HasForeignKey(e => e.OrganizerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // EventImage Configuration
            modelBuilder.Entity<EventImage>(entity =>
            {
                entity.HasKey(ei => ei.Id);
                
                entity.HasOne(ei => ei.Event)
                      .WithMany(e => e.Images)
                      .HasForeignKey(ei => ei.EventId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // EventRegistration Configuration
            modelBuilder.Entity<EventRegistration>(entity =>
            {
                entity.HasKey(er => er.Id);
                
                entity.HasOne(er => er.Event)
                      .WithMany(e => e.Registrations)
                      .HasForeignKey(er => er.EventId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(er => er.Student)
                      .WithMany(s => s.EventRegistrations)
                      .HasForeignKey(er => er.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(er => new { er.EventId, er.StudentId }).IsUnique();
            });

            // EventSchedule Configuration
            modelBuilder.Entity<EventSchedule>(entity =>
            {
                entity.HasKey(es => es.Id);
                
                entity.HasOne(es => es.Event)
                      .WithMany(e => e.Schedules)
                      .HasForeignKey(es => es.EventId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Contest Configuration
            modelBuilder.Entity<Contest>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
                
                entity.HasOne(c => c.Category)
                      .WithMany(cat => cat.Contests)
                      .HasForeignKey(c => c.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(c => c.Organizer)
                      .WithMany(u => u.OrganizedContests)
                      .HasForeignKey(c => c.OrganizerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ContestImage Configuration
            modelBuilder.Entity<ContestImage>(entity =>
            {
                entity.HasKey(ci => ci.Id);
                
                entity.HasOne(ci => ci.Contest)
                      .WithMany(c => c.Images)
                      .HasForeignKey(ci => ci.ContestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ContestRegistration Configuration
            modelBuilder.Entity<ContestRegistration>(entity =>
            {
                entity.HasKey(cr => cr.Id);
                
                entity.HasOne(cr => cr.Contest)
                      .WithMany(c => c.ContestRegistrations)
                      .HasForeignKey(cr => cr.ContestId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(cr => cr.Leader)
                      .WithMany(s => s.LedContestRegistrations)
                      .HasForeignKey(cr => cr.LeaderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ContestMember Configuration
            modelBuilder.Entity<ContestMember>(entity =>
            {
                entity.HasKey(cm => cm.Id);
                
                entity.HasOne(cm => cm.ContestRegistration)
                      .WithMany(cr => cr.Members)
                      .HasForeignKey(cm => cm.ContestRegistrationId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(cm => cm.Student)
                      .WithMany(s => s.ContestMembers)
                      .HasForeignKey(cm => cm.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(cm => new { cm.ContestRegistrationId, cm.StudentId }).IsUnique();
            });
        }
    }
}
