using System.Net.Http.Json;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Services
{
    // Implementation of HttpClient service for REST API communication
    public class ServiceRequestApiService : IServiceRequestApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ServiceRequestApiService(
            HttpClient httpClient, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        // Resolves the current base URL dynamically from the active request context
        private string GetApiUrl(string endpoint)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl;

            if (request != null)
            {
                baseUrl = $"{request.Scheme}://{request.Host}";
            }
            else
            {
                baseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000";
            }

            return $"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
        }

        // GET: api/servicerequests
        public async Task<List<ServiceRequest>> GetAllRequestsAsync()
        {
            try
            {
                var url = GetApiUrl("api/servicerequests");
                var response = await _httpClient.GetFromJsonAsync<List<ServiceRequest>>(url);
                return response ?? new List<ServiceRequest>();
            }
            catch (Exception)
            {
                return new List<ServiceRequest>();
            }
        }

        // GET: api/servicerequests/{id}
        public async Task<ServiceRequest?> GetRequestByIdAsync(int id)
        {
            try
            {
                var url = GetApiUrl($"api/servicerequests/{id}");
                return await _httpClient.GetFromJsonAsync<ServiceRequest>(url);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // POST: api/servicerequests
        public async Task<bool> CreateRequestAsync(ServiceRequest request)
        {
            try
            {
                var url = GetApiUrl("api/servicerequests");
                var response = await _httpClient.PostAsJsonAsync(url, request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // PUT: api/servicerequests/{id}
        public async Task<bool> UpdateRequestAsync(int id, ServiceRequest request)
        {
            try
            {
                var url = GetApiUrl($"api/servicerequests/{id}");
                var response = await _httpClient.PutAsJsonAsync(url, request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // PUT: api/servicerequests/{id}/status
        public async Task<bool> UpdateStatusAsync(int id, string status, string? remarks)
        {
            try
            {
                var url = GetApiUrl($"api/servicerequests/{id}/status");
                var statusDto = new StatusUpdateDto { Status = status, AdminRemarks = remarks };
                var response = await _httpClient.PutAsJsonAsync(url, statusDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // DELETE: api/servicerequests/{id}
        public async Task<bool> DeleteRequestAsync(int id)
        {
            try
            {
                var url = GetApiUrl($"api/servicerequests/{id}");
                var response = await _httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
