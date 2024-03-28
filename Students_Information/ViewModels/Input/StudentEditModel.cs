using Students_Information.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Students_Information.ViewModels.Input
{
    public class StudentEditModel
    {
        public int StudentId { get; set; }
        [Required, StringLength(50)]
        public string StudentName { get; set; } = default!;
        [Required]
        public int Age { get; set; }
        [Required, EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        public IFormFile? Picture { get; set; } = default!;
        public bool IsRegular { get; set; }
    }
}
