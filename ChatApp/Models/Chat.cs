using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<ChatUser> ChatUsers { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
