using EDBS_server.DTOs.Requests;
using EDBS_server.DTOs.Responses;

namespace EDBS_server.Services
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
    }
}