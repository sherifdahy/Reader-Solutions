using Reader.BLL.Contracts.User.Requests;
using Reader.BLL.Contracts.User.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL.Contracts.Group.Responses;

public class GroupResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ClientResponse> Clients { get; set; } = default!;
}
