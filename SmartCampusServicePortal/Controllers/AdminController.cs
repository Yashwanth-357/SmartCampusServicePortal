using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;
using SmartCampusServicePortal.Services;

namespace SmartCampusServicePortal.Controllers
{
    // Admin Controller - Manages student requests, status updates, and filtering
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceRequestApiService _apiService;

        public AdminController(ApplicationDbContext context, IServiceRequestApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        // GET: Admin
        // Displays all requests with category and status filtering
        public async Task<IActionResult> Index(string? category, string? status)
        {
            var query = _context.ServiceRequests.AsQueryable();

            // Filter by Category
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                query = query.Where(r => r.Category == category);
            }

            // Filter by Status
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(r => r.Status == status);
            }

            var requests = await query.OrderByDescending(r => r.SubmissionDate).ToListAsync();

            ViewBag.SelectedCategory = category ?? "All";
            ViewBag.SelectedStatus = status ?? "All";

            return View(requests);
        }

        // GET: Admin/UpdateStatus/5
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            var dto = new StatusUpdateDto
            {
                Status = request.Status,
                AdminRemarks = request.AdminRemarks
            };

            ViewBag.Request = request;
            return View(dto);
        }

        // POST: Admin/UpdateStatus/5
        // Updates the status and remarks using the API service
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, StatusUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                // Call API service to perform status update
                bool success = await _apiService.UpdateStatusAsync(id, dto.Status, dto.AdminRemarks);
                if (success)
                {
                    TempData["SuccessMessage"] = $"Status for Request #{id} updated to '{dto.Status}' successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update status through API.");
            }

            var request = await _context.ServiceRequests.FindAsync(id);
            ViewBag.Request = request;
            return View(dto);
        }
    }
}
