using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL.Contracts.Message;

public class MessageResponse
{
    public string From { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public MessageBodyResponse Body { get; set; } = default!;
    public DateTime DateTime { get; set; }
}
