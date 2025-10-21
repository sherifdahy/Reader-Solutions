using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Reader.BLL.Contracts.Message;

public class MessageBodyResponse
{
    public string Topic { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Partner { get; set; } = string.Empty;
    public string RecordingLink { get; set; } = string.Empty;
}
