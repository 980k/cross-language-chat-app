using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossLangChat.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Username is required.")]
        public string ? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string ? Password { get; set; }
    }
}