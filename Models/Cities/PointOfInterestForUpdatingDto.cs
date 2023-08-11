using System.ComponentModel.DataAnnotations;

namespace CityInfo.Models.Cities
{
    public class PointOfInterestForUpdatingDto
    {
        [Required(ErrorMessage = "You should provide a name value"), MaxLength(50, ErrorMessage = "too long")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
