using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Controllers
{
    // Home Controller - Serves the main Portal Dashboard
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /
        // Displays the Campus Dashboard with summary statistics and recent requests
        public async Task<IActionResult> Index()
        {
            var totalRequests = await _context.ServiceRequests.CountAsync();
            var pendingRequests = await _context.ServiceRequests.CountAsync(r => r.Status == "Pending");
            var inProgressRequests = await _context.ServiceRequests.CountAsync(r => r.Status == "In Progress");
            var completedRequests = await _context.ServiceRequests.CountAsync(r => r.Status == "Completed");
            var totalStudents = await _context.Students.CountAsync();

            var recentRequests = await _context.ServiceRequests
                .OrderByDescending(r => r.SubmissionDate)
                .Take(5)
                .ToListAsync();

            var dashboardModel = new DashboardViewModel
            {
                TotalRequests = totalRequests,
                PendingRequests = pendingRequests,
                InProgressRequests = inProgressRequests,
                CompletedRequests = completedRequests,
                TotalStudents = totalStudents,
                RecentRequests = recentRequests
            };

            return View(dashboardModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
