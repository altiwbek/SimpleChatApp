﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class ChatUser
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
