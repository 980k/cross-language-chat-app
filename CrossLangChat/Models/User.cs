using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossLangChat.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Username is too short")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username contains invalid characters")]
        public string ? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Password is too short")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Password contains invalid characters")]
        public string ? Password { get; set; }

        public ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
    }
}
