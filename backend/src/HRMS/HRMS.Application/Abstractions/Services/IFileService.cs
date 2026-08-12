namespace HRMS.Application.Abstractions.Services
{
    public interface IFileService
    {
        public Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, bool isImage, CancellationToken cancellationToken);

    }
}
