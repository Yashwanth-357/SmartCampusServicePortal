using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;
using SmartCampusServicePortal.Services;

namespace SmartCampusServicePortal.Controllers
{
    // Service Requests Controller - Demonstrates MVC calling REST API via HttpClient
    public class ServiceRequestsController : Controller
    {
        private readonly IServiceRequestApiService _apiService;
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(
            IServiceRequestApiService apiService,
            ApplicationDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }

        // GET: ServiceRequests
        // Retrieves all requests via REST API HttpClient service
        public async Task<IActionResult> Index()
        {
            var requests = await _apiService.GetAllRequestsAsync();
            return View(requests);
        }

        // GET: ServiceRequests/Details/5
        // Retrieves request details via REST API HttpClient service
        public async Task<IActionResult> Details(int id)
        {
            var request = await _apiService.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            // Populate student dropdown for convenience
            var students = await _context.Students.OrderBy(s => s.FullName).ToListAsync();
            ViewBag.Students = new SelectList(students, "FullName", "FullName");

            return View();
        }

        // POST: ServiceRequests/Create
        // Submits a new campus service request by calling the REST API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                request.SubmissionDate = DateTime.Now;
                request.Status = "Pending";

                // Auto-fill student roll number if matched in database
                if (string.IsNullOrEmpty(request.StudentRollNo))
                {
                    var student = await _context.Students
                        .FirstOrDefaultAsync(s => s.FullName == request.StudentName);
                    if (student != null)
                    {
                        request.StudentRollNo = student.StudentIdNumber;
                    }
                }

                bool success = await _apiService.CreateRequestAsync(request);
                if (success)
                {
                    TempData["SuccessMessage"] = "Your campus service request has been submitted successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Unable to create service request via API. Please check API status.");
            }

            var students = await _context.Students.OrderBy(s => s.FullName).ToListAsync();
            ViewBag.Students = new SelectList(students, "FullName", "FullName");
            return View(request);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var request = await _apiService.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: ServiceRequests/Edit/5
        // Updates the service request via REST API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool success = await _apiService.UpdateRequestAsync(id, request);
                if (success)
                {
                    TempData["SuccessMessage"] = "Service request updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Unable to update request via API.");
            }

            return View(request);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _apiService.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: ServiceRequests/Delete/5
        // Deletes the service request via REST API
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool success = await _apiService.DeleteRequestAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = $"Service request #{id} deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to delete service request #{id}.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Track
        // Track request status by ID or Student Name
        public async Task<IActionResult> Track(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(null);
            }

            query = query.Trim();

            // Try matching by ID first
            if (int.TryParse(query, out int requestId))
            {
                var single = await _apiService.GetRequestByIdAsync(requestId);
                if (single != null)
                {
                    return View(new List<ServiceRequest> { single });
                }
            }

            // Otherwise match by student name or roll number
            var all = await _apiService.GetAllRequestsAsync();
            var matches = all.Where(r => 
                r.StudentName.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                (r.StudentRollNo != null && r.StudentRollNo.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!matches.Any())
            {
                ViewBag.ErrorMessage = $"No requests found matching '{query}'.";
            }

            return View(matches);
        }
    }
}
