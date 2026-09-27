using MediVora.Interfaces;

namespace MediVora.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        private readonly string[] _allowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        private const long MaxFileSize = 1 * 1024 * 1024; // 2 MB
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            if (file.Length > MaxFileSize)
            {
                throw new ArgumentException("File size cannot exceed 2 MB.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Only JPG, JPEG, PNG and WEBP files are allowed.");
            }

            // Generate unique file name
            var fileName = $"{Guid.NewGuid()}{extension}";

            // wwwroot/uploads/doctors
            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrEmpty(webRootPath))
            {
                throw new InvalidOperationException("wwwroot folder is not configured.");
            }

            var uploadFolder = Path.Combine(webRootPath,"uploads",folderName);

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var filePath = Path.Combine(uploadFolder, fileName);

            // Save file
            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            // Return relative path
            return Path.Combine("uploads", folderName, fileName).Replace("\\", "/");
        }


        public bool DeleteFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }

            var fullPath = Path.Combine(_environment.WebRootPath, filePath);

            if (!File.Exists(fullPath))
            {
                return false;
            }

            File.Delete(fullPath);

            return true;
        }
    }
}
