namespace Ecommerce.WebUI.Utils
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile formFile, string folder = "Img/Products")
        {
            if (formFile == null || formFile.Length == 0)
                return string.Empty;

            string uploadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                folder
            );

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string fileName = Guid.NewGuid() + Path.GetExtension(formFile.FileName);

            string fullPath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await formFile.CopyToAsync(stream);

            return fileName;
        }

        public static bool FileRemover(string fileName, string filepath = "/Img/")
        {
            if (string.IsNullOrEmpty(fileName)) return false;

            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filepath.TrimStart('/'), fileName);
            if (File.Exists(directory))
            {
                File.Delete(directory);
                return true;
            }

            return false;
        }
    }
}