using Doctor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Doctor.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _env;

        public FileService(IHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Faylı serverə yükləyir və qovluqda saxlayır.
        /// </summary>
        public async Task<string?> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            // wwwroot/uploads/{folder}
            var uploadRoot = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads");
            var targetFolder = Path.Combine(uploadRoot, folder);

            // qovluq yoxdursa, yarat
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(targetFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // DB və ya client-ə qayıdan yol (wwwroot-suz)
            return $"/uploads/{folder}/{fileName}";
        }

        /// <summary>
        /// Faylı silir (əgər mövcuddursa).
        /// </summary>
        public void Delete(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            // Məsələn: /uploads/diets/abc.txt
            var normalizedPath = filePath.TrimStart('/')
                                         .Replace("/", Path.DirectorySeparatorChar.ToString());

            var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot", normalizedPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// Alternativ upload metodu.
        /// </summary>
        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var path = Path.Combine("uploads", folder, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/" + path.Replace("\\", "/"); // 🔥 BURDA FIX
        }

    }
}
