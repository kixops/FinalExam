using System.ComponentModel.DataAnnotations;

namespace ASP_FinalExam_Net6.Models
{
    public partial class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public bool IsManager { get; set; }

        public int? DepartmentId { get; set; }

        public virtual Department Dept { get; set; }
    }
}
