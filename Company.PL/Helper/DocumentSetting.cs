using Humanizer;

namespace Company.PL.Helper
{
    public static class DocumentSetting
    {
        public static string Upload(IFormFile file , string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\Files\\{folderName}");

            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            string filePath = Path.Combine(folderPath, fileName);

            using var FileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo( FileStream );

            return fileName;
        }

        public static void Delete(string fileName, string folderName) 
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\Files\\{folderName}" , fileName);

            if(File.Exists(filePath)) 
                File.Delete(filePath);
        }
    }
}
