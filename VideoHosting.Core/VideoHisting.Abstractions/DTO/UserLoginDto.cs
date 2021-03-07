
using System;

namespace VideoHosting.Abstractions.Dto
{
    public class UserLoginDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}
