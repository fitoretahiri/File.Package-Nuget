using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace File.Package.FileService
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env) 
        {
            _env= env;
        }
        public Tuple<int, string> SaveImage(IFormFile file)
        {
            try
            {
                var contentPath = this._env.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads", "Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(file.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, ex.Message);
            }
        }

        public Tuple<int, string> SaveImageAndVideo(IFormFile file)
        {
            try
            {
                var contentPath = this._env.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads", "Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(file.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".mp3", ".mp4" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, ex.Message);
            }
        }

        public async Task<string> SavePdfAsync(IFormFile file)
        {
            var fiveMegaBytes = 5 * 1024 * 1024;
            var pdfType = "application/pdf";

            if (file.Length > fiveMegaBytes || file.ContentType != pdfType)
            {
                return null;
            }

            var resumeUrl = Guid.NewGuid().ToString() + ".pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "pdfs", resumeUrl);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return resumeUrl;
        }


        public bool DeleteFile(string fileName)
        {
            try
            {

                var contentPath = this._env.ContentRootPath;
                var path = "";
                if (fileName.Contains(".pdf"))
                {
                    path = Path.Combine(contentPath, "Uploads", "pdfs", fileName);
                }
                path = Path.Combine(contentPath, "Uploads", "Images", fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public byte[] DownloadPdfFile(string url)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "pdfs", url);

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("File Not Found");
            }

            var pdfBytes = System.IO.File.ReadAllBytes(filePath);
            return pdfBytes;
        }

    }
}
