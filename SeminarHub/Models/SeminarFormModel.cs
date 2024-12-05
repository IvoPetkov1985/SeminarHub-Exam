using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.Common.DataConstants;

namespace SeminarHub.Models
{
    public class SeminarFormModel
    {
        [Required]
        [StringLength(TopicMaxLength, MinimumLength = TopicMinLength)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [StringLength(LecturerMaxLength, MinimumLength = LecturerMinLength)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [StringLength(DetailsMaxLength, MinimumLength = DetailsMinLength)]
        public string Details { get; set; } = string.Empty;

        [Required]
        [RegularExpression(DateTimeRegex, ErrorMessage = DateTimeFormatErrorMsg)]
        public string DateAndTime { get; set; } = string.Empty;

        [Range(DurationMin, DurationMax)]
        public int? Duration { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
