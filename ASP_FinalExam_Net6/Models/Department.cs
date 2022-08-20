using System.ComponentModel.DataAnnotations;

namespace ASP_FinalExam_Net6.Models
{
    public partial class Department
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int EmployeeCount { get; set; } = 0;

    }
}
