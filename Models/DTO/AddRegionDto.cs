using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class AddRegionDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="Code has to be minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be minimum of 3 characters")]

        public string code { get; set; }
        [Required]
        public string Name { get; set; }
        public string? RegionImageURL { get; set; }
    }
}
