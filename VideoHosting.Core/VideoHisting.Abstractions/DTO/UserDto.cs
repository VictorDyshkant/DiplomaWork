using System;

namespace VideoHosting.Abstractions.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Faculty { get; set; }

        public string Group { get; set; }

        public bool Admin { get; set; }

        public bool DoSubscribed { get; set; }

        public int Subscribers { get; set; }

        public string PhotoPath { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string Country { get; set; }

        public bool Sex { get; set; }

        public int Subscriptions { get; set; }
    }
}
