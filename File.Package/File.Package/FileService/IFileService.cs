using Microsoft.AspNetCore.Http;

namespace File.Package.FileService
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile file);
        bool DeleteFile(string fileName);
        byte[] DownloadPdfFile(string url);
        Task<string> SavePdfAsync(IFormFile file);
        Tuple<int, string> SaveImageAndVideo(IFormFile file);
    }
}
