using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.Entities.Entities;

public class GroupClient
{
    public int GroupId { get; set; }
    public int ClientId { get; set; }

    public Client Client { get; set; } = default!;
    public Group Group { get; set; } = default!;
}
