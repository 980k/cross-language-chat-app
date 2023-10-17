using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CrossLangChat.Models
{
    public class ChatRoom 
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Room name is required.")]
        public string ? RoomName { get; set; }

        public ICollection<User> ? Users { get; set; } = new List<User>();

        public ICollection<Message> ? Messages { get; set; } = new List<Message>();
    }
}