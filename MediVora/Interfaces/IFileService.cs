namespace MediVora.Interfaces
{
    public interface IFileService
    {
        public Task<string> UploadFileAsync(IFormFile file, string folderName);
        public bool DeleteFile(string filePath);
    }
}
