using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HRMS.Application.Abstractions.Services;
using HRMS.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace HRMS.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly Cloudinary _cloudinary;

        public FileService(IOptions<CloudinarySettings> options)
        {
            var cloudinarySettings = options.Value;

            if (string.IsNullOrEmpty(cloudinarySettings.CloudName) ||
                string.IsNullOrEmpty(cloudinarySettings.ApiKey) ||
                string.IsNullOrEmpty(cloudinarySettings.ApiSecret))
            {
                throw new ArgumentException("Cloudinary settings are not properly configured.");
            }

            var account = new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, bool isImage, CancellationToken cancellationToken)
        {
            UploadResult uploadResult;
            string safeFileName = GenerateSafeFileName(fileName, folder);
            if (isImage)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(safeFileName, fileStream),
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);
            }
            else
            {
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(safeFileName, fileStream),
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Cloudinary upload failed: {uploadResult.Error?.Message}");

            return uploadResult.SecureUrl.AbsoluteUri;
        }

        private string GenerateSafeFileName(string originalFileName, string folderName)
        {
            var extension = Path.GetExtension(originalFileName).ToLowerInvariant();

            return $"{folderName}_{Guid.NewGuid()}{extension}";
        }
    }
}
