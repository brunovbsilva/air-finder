﻿using AirFinder.Application.Common;
using AirFinder.Domain.Users;

namespace AirFinder.Application.Users.Models.Response
{
    public class LoginResponse : BaseResponse
    {
        public User? User { get; set; }
    }
}