using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossLangChat.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ? Content { get; set; }

        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Timestamp { get; set; }

        [ForeignKey("SenderId")]
        public int SenderId { get; set; }
        public User ? Sender { get; set; }

        [ForeignKey("ChatRoomId")]
        public int ChatRoomId { get; set; }
        public ChatRoom ? ChatRoom { get; set; }
    }
}
