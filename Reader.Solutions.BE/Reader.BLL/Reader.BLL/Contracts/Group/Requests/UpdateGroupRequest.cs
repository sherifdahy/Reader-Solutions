using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL.Contracts.Group.Requests;

public class UpdateGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public List<int> ClientIds { get; set; } = [];
}
