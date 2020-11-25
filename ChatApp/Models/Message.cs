using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        
        [Required]
        public string TextContent { get; set; }
       
        [Required]
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
