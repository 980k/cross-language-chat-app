using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossLangChat.Models
{
    public class Message
    {
        public Message(User Sender, string Content, ChatRoom ChatRoom) {
            this.Sender = Sender;
            this.Content = Content;
            this.ChatRoom = ChatRoom;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        [Required]
        public string ? Content {get; set;}

        [Required]
        public User Sender { get; set; }

        [Required]
        public ChatRoom ChatRoom {get; set; }
    }
}