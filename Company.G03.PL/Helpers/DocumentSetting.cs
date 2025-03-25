using System.Drawing;

namespace Company.G03.PL.Helpers
{
    public static class DocumentSetting
    {

        //Upload
        //ImageName
        //public static string UploadFile(IFormFile file, string folderName) {

        //    //Get Folder Location
        //    //string folerPath = "E:\\ASP.Net\\Session 4\\assigment\\Company.G03.PL\\Company.G03.PL\\wwwroot\\file\\images\\"+folderName;

        //    //var folderPath=  Directory.GetCurrentDirectory() + "\\wwwroot\\file\\"+folderName;

        //    var folderPath = Path.Combine(Directory.GetCurrentDirectory() , @"wwwroot\file\" , folderName);


        //    //2-  Get File Name and make it Unique
        //    var fileName= $"{Guid.NewGuid()}{file.FileName}";
        //    // File Path

        //    var filePath = Path.Combine(folderPath, fileName);
        //    var fileStream = new FileStream(filePath, FileMode.Create);
        //    file.CopyTo(fileStream);
        //    return fileName;
        //}
        public static string UploadFile(IFormFile file, string folderName)
        {
            // Ensure directory exists
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath); // Create directory if it doesn't exist
            }

            // Generate a unique file name to avoid overwriting
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            // Combine folder path and file name
            var filePath = Path.Combine(folderPath, fileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }


        //delete
        public static void DeleteFile(string fileName, string folderName) {


            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\file\", folderName, fileName);

            if (File.Exists(filePath)) { 
            
            File.Delete(filePath);
            }
        


        }
    }
}
