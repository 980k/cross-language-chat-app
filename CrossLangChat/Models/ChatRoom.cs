using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CrossLangChat.Models
{
    public class ChatRoom 
    {
        public ChatRoom(string roomName, ICollection<User> participants)  
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                throw new ArgumentException("Room name cannot be null or empty.", nameof(roomName));
            }

            if (participants == null || participants.Count < 2)
            {
                throw new ArgumentException("A chat room must have at least two participants.", nameof(participants));
            }

            RoomName = roomName;
            Participants = new List<User>(participants);
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        [Required]
        public string RoomName { get; set; }

         // Navigation property representing the users participating in this chatroom
        public ICollection<User> Participants { get; set; }

        // Navigation property representing the messages sent in this chatroom
        public ICollection<Message> ? Messages { get; set; }
    }
}