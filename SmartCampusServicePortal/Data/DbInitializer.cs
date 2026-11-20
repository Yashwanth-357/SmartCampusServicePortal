using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Data
{
    // Helper to seed initial sample data for demonstration
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if Students already exist
            if (!context.Students.Any())
            {
                var students = new List<Student>
                {
                    new Student
                    {
                        StudentIdNumber = "STU101",
                        FullName = "Rahul Sharma",
                        Email = "rahul.sharma@campus.edu",
                        PhoneNumber = "9876543210",
                        Department = "Computer Science",
                        RegisteredDate = DateTime.Now.AddDays(-20)
                    },
                    new Student
                    {
                        StudentIdNumber = "STU102",
                        FullName = "Priya Patel",
                        Email = "priya.patel@campus.edu",
                        PhoneNumber = "9876543211",
                        Department = "Information Technology",
                        RegisteredDate = DateTime.Now.AddDays(-15)
                    },
                    new Student
                    {
                        StudentIdNumber = "STU103",
                        FullName = "Amit Verma",
                        Email = "amit.verma@campus.edu",
                        PhoneNumber = "9876543212",
                        Department = "Mechanical Engineering",
                        RegisteredDate = DateTime.Now.AddDays(-10)
                    },
                    new Student
                    {
                        StudentIdNumber = "STU104",
                        FullName = "Sneha Rao",
                        Email = "sneha.rao@campus.edu",
                        PhoneNumber = "9876543213",
                        Department = "Electrical Engineering",
                        RegisteredDate = DateTime.Now.AddDays(-5)
                    }
                };

                context.Students.AddRange(students);
                context.SaveChanges();
            }

            // Check if Service Requests already exist
            if (!context.ServiceRequests.Any())
            {
                var requests = new List<ServiceRequest>
                {
                    new ServiceRequest
                    {
                        StudentName = "Rahul Sharma",
                        StudentRollNo = "STU101",
                        Category = "IT Support",
                        Description = "Campus Wi-Fi is disconnecting frequently in Computer Lab 3.",
                        SubmissionDate = DateTime.Now.AddDays(-3),
                        Status = "Pending",
                        AdminRemarks = null
                    },
                    new ServiceRequest
                    {
                        StudentName = "Priya Patel",
                        StudentRollNo = "STU102",
                        Category = "Hostel",
                        Description = "Hot water supply is not working on 2nd Floor, Block B.",
                        SubmissionDate = DateTime.Now.AddDays(-2),
                        Status = "In Progress",
                        AdminRemarks = "Plumber assigned to inspect Block B heater."
                    },
                    new ServiceRequest
                    {
                        StudentName = "Amit Verma",
                        StudentRollNo = "STU103",
                        Category = "Library",
                        Description = "Digital library portal access is showing invalid credentials error.",
                        SubmissionDate = DateTime.Now.AddDays(-5),
                        Status = "Completed",
                        AdminRemarks = "Library portal account credentials reset and verified."
                    },
                    new ServiceRequest
                    {
                        StudentName = "Sneha Rao",
                        StudentRollNo = "STU104",
                        Category = "Maintenance",
                        Description = "Ceiling fan speed regulator in Classroom 204 is broken.",
                        SubmissionDate = DateTime.Now.AddDays(-1),
                        Status = "Pending",
                        AdminRemarks = null
                    }
                };

                context.ServiceRequests.AddRange(requests);
                context.SaveChanges();
            }
        }
    }
}
