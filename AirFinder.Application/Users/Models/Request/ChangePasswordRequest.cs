﻿namespace AirFinder.Application.Users.Models.Request
{
    public class ChangePasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
