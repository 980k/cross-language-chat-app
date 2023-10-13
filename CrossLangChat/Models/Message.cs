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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        [Required]
        public string ? Content {get; set;}

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}