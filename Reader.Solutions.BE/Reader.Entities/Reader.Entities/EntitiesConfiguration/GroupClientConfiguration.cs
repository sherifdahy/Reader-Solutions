using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reader.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.Entities.EntitiesConfiguration
{
    public class GroupClientConfiguration : IEntityTypeConfiguration<GroupClient>
    {
        public void Configure(EntityTypeBuilder<GroupClient> builder)
        {
            builder.HasKey(x => new { x.GroupId, x.ClientId });
        }
    }
}
