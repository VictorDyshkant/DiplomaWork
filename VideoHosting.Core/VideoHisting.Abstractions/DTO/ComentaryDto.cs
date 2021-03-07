using System;
using System.ComponentModel.DataAnnotations;

namespace VideoHosting.Abstractions.Dto
{
    public class CommentaryDto
    {
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime DayOfCreation { get; set; }

        [Required]
        public Guid VideoId { get; set; }
    }
}
