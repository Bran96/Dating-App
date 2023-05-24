﻿using System.ComponentModel.DataAnnotations;

namespace DatingAppApi.DTO_s
{
    public class RegisterDto
    {
        // These are for validaton inside the brackets '[]'
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;
    }
}
