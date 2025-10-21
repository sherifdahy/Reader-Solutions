﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.Entities.Entities;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AppPassword { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public ICollection<GroupClient> GroupClients { get; set; } = new HashSet<GroupClient>();
}
