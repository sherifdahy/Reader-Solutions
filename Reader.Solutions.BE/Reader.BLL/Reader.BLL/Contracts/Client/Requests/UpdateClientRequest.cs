using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL.Contracts.Client.Requests;

public class UpdateClientRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AppPassword { get; set; } = string.Empty;
    public List<int> GroupIds { get; set; } = new List<int>();
}
