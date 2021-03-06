﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class User : IdentityUser

    {
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<ChatUser> ChatUsers { get; set; }

        public IEnumerable<Message> Messages { get; set; }

    }
}
