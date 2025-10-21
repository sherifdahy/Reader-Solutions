using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reader.Entities.Abstractions.Consts;
using Reader.Entities.Entities;

namespace NOTE.Solutions.Entities.EntitiesConfiguration;
public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(new ApplicationRole()
        {
            Id = DefaultRoles.AdminRoleId,
            Name = DefaultRoles.Admin,
            NormalizedName = DefaultRoles.Admin.ToUpper(),
            ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp,
        });
    }
}
