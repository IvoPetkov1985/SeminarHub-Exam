using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SeminarHub.Data.Common.DataConstants;

namespace SeminarHub.Data.DataModels
{
    [Comment("The seminar with its properties")]
    public class Seminar
    {
        [Key]
        [Comment("Seminar identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(TopicMaxLength)]
        [Comment("Seminar topic")]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [MaxLength(LecturerMaxLength)]
        [Comment("Lecturer names")]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(DetailsMaxLength)]
        [Comment("Details about the seminar")]
        public string Details { get; set; } = string.Empty;

        [Required]
        [Comment("Organizer (user) identifier")]
        public string OrganizerId { get; set; } = string.Empty;

        [ForeignKey(nameof(OrganizerId))]
        public IdentityUser Organizer { get; set; } = null!;

        [Required]
        [Comment("Date and time when the seminar takes place")]
        public DateTime DateAndTime { get; set; }

        [Comment("Duration of the seminar in minutes")]
        public int? Duration { get; set; }

        [Required]
        [Comment("Category identifier")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public IEnumerable<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();
    }
}
