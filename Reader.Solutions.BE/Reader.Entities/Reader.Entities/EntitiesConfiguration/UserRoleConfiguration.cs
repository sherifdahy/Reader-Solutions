using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reader.Entities.Abstractions.Consts;

namespace Reader.Entities.EntitiesConfiguration;
public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
    {
        builder.HasData(new IdentityUserRole<int>()
        {
            RoleId = DefaultRoles.AdminRoleId,
            UserId = DefaultUsers.AdminId,
        });
    }
}
