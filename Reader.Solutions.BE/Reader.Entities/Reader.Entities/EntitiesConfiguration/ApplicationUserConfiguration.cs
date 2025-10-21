using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reader.Entities.Abstractions.Consts;
using Reader.Entities.Entities;
namespace NOTE.Solutions.Entities.EntitiesConfiguration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(x => new { x.Email }).IsUnique();
        builder.OwnsMany(x => x.RefreshTokens).WithOwner();

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(new ApplicationUser()
        {
            Id = DefaultUsers.AdminId,
            UserName = DefaultUsers.AdminEmail,
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword),
        });
    }
}
