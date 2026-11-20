using System.ComponentModel.DataAnnotations;

namespace SmartCampusServicePortal.Models
{
    // Represents a campus service request submitted by a student
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Student Name is required")]
        [Display(Name = "Student Name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string StudentName { get; set; } = string.Empty;

        [Display(Name = "Student ID / Roll No")]
        [StringLength(20)]
        public string? StudentRollNo { get; set; }

        [Required(ErrorMessage = "Please select a service category")]
        [Display(Name = "Service Category")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // IT Support, Hostel, Library, Maintenance

        [Required(ErrorMessage = "Please provide a description of the issue")]
        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Submission Date")]
        [DataType(DataType.DateTime)]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Status")]
        [StringLength(30)]
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed

        [Display(Name = "Admin Remarks")]
        [StringLength(250)]
        public string? AdminRemarks { get; set; }
    }
}
