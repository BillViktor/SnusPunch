﻿namespace SnusPunch.Shared.Models.Auth
{
    public class VerifyEmailRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
