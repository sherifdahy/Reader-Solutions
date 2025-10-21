using Mapster;
using Reader.BLL.Contracts.Group.Responses;
using Reader.BLL.Contracts.User.Responses;
using Reader.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Group, GroupResponse>().Map(dest => dest.Clients, src => src.GroupClients.Select(x => new Client()
        {
            Id = x.Client.Id,
            Name = x.Client.Name,
            Email = x.Client.Email,
            AppPassword = x.Client.AppPassword
        }));
    }
}
