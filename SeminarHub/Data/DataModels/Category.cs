using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.Common.DataConstants;

namespace SeminarHub.Data.DataModels
{
    [Comment("Seminar category")]
    public class Category
    {
        [Key]
        [Comment("Category identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        [Comment("Category name")]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Seminar> Seminars { get; set; } = new List<Seminar>();
    }
}
