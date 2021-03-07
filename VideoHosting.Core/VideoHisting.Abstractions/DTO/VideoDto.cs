using System;
using System.ComponentModel.DataAnnotations;

namespace VideoHosting.Abstractions.Dto
{
    public class VideoDto
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public DateTime DayOfCreation { get; set; }

        public int Views { get; set; }

        public string VideoPath { get; set; }

        public string PhotoPath { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string Description { get; set; }

        public bool Liked { get; set; }

        public bool Disliked { get; set; }
    }
}
