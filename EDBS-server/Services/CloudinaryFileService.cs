using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EDBS_server.Settings;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http; // Thêm dòng này để C# nhận diện IFormFile

namespace EDBS_server.Services
{
    public class CloudinaryFileService : IFileService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryFileService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        // CHỈ GIỮ LẠI HÀM UPLOAD 1 FILE DUY NHẤT
        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không được để trống.");

            // Ép Cloudinary cho vào đúng thư mục mình muốn (vd: edbs/avatars)
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = $"edbs/{folderName}", // Phân loại thư mục trên Cloudinary
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            // Trả về link ảnh xịn xò (HTTPS) do Cloudinary cung cấp
            return uploadResult.SecureUrl.ToString();
        }
    }
}