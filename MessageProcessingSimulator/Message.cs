using System;
using System.Collections.Generic;
using System.Text;

namespace MessageProcessingSimulator
{
    public class Message
    {
        public long SeqNum { get; set; }
        public string MessageType { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
    }
}
