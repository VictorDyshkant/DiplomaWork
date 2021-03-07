using System.ComponentModel.DataAnnotations;

namespace VideoHosting.Abstractions.DTO
{
    public class VideoAddDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string VideoPath { get; set; }

        public string PhotoPath { get; set; }

        public string Description { get; set; }

    }
}
