using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CrossLangChat.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string ? Content { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string? SenderUsername { get; set; } // Store the username directly as a string

        [ForeignKey("ChatRoom")]
        public int ChatRoomId { get; set; } // Foreign key for ChatRoom

        public ChatRoom ? ChatRoom { get; set; } // Navigation property to the associated ChatRoom
    }
}
