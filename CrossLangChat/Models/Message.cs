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

        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public User ? Sender { get; set; }

        public int ChatRoomId { get; set; }

        [ForeignKey("ChatRoomId")]
        public ChatRoom ? ChatRoom { get; set; }
    }
}
