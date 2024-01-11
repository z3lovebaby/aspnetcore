using CRUD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class CRUDContext : IdentityDbContext<AppUsers>
{
    public CRUDContext(DbContextOptions<CRUDContext> options) : base(options)
    {
    }

    public DbSet<ExamScore> ExamScores { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Course> Course { get; set; }
    public virtual DbSet<UserRefreshTokens> UserRefreshToken { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Additional model configuration, role definition, admin user creation, and role assignment can be done here.

        var readerRoleId = "28d65a5b-a7db-4850-b380-83591f7d7531";
        var writerRoleId = "9740f16c-24a1-4224-a7be-1bb00b7c6892";

        // Create Reader and Writer Role
        var roles = new List<IdentityRole>
        {
            new IdentityRole()
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp = readerRoleId
            },
            new IdentityRole()
            {
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp = writerRoleId
            }
        };

        // Seed the roles
        builder.Entity<IdentityRole>().HasData(roles);

        // Create an Admin User
        var adminUserId = "edc267ec-d43c-4e3b-8108-a1a1f819906d";
        var admin = new AppUsers()
        {
            Id = adminUserId,
            UserName = "admin@crud.com",
            Email = "admin@crud.com",
            NormalizedEmail = "admin@crud.com".ToUpper(),
            NormalizedUserName = "admin@crud.com".ToUpper()
        };

        admin.PasswordHash = new PasswordHasher<AppUsers>().HashPassword(admin, "Admin@123");

        builder.Entity<AppUsers>().HasData(admin);

        // Give Roles To Admin
        var adminRoles = new List<IdentityUserRole<string>>()
        {
            new()
            {
                UserId = adminUserId,
                RoleId = readerRoleId
            },
            new()
            {
                UserId = adminUserId,
                RoleId = writerRoleId
            }
        };

        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}
