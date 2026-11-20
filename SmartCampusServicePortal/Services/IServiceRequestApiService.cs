using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Services
{
    // Interface for communicating with the REST API using HttpClient
    public interface IServiceRequestApiService
    {
        Task<List<ServiceRequest>> GetAllRequestsAsync();
        Task<ServiceRequest?> GetRequestByIdAsync(int id);
        Task<bool> CreateRequestAsync(ServiceRequest request);
        Task<bool> UpdateRequestAsync(int id, ServiceRequest request);
        Task<bool> UpdateStatusAsync(int id, string status, string? remarks);
        Task<bool> DeleteRequestAsync(int id);
    }
}
