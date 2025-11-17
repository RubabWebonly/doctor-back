using Microsoft.AspNetCore.Http;

namespace Doctor.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(IFormFile file, string folder);
        Task<string> UploadAsync(IFormFile file, string folder);
        void Delete(string filePath);
    }
}
