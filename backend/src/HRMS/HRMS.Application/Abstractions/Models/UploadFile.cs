namespace HRMS.Application.Abstractions.Models
{
    public sealed record UploadedFile(
        Stream Content,
        string FileName,
        string ContentType,
        long Length);
}
