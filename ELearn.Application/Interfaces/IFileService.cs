using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Domain.Entities;

namespace ELearn.Application.Interfaces
{
    public interface IFileService
    {
        Task<Response<FileDTO>> GetByIdAsync(int Id);
        Task<Response<FileDTO>> GetByFileNameAsync(string fileName);
        Task<Response<ICollection<FileDTO>>> GetAllFilesAsync();
        Task<Response<FileDTO>> UploadFileAsync(UploadFileDTO fileDTO);
        Task<byte[]> DownloadFileAsync(DownloadFileDTO downloadFileDTO);
        Task<Response<FileDTO>> DeleteAsync(int Id);
        Task<string>GetFileType(string fileName);
        Task<Response<List<int>>> GetFilesByPostId(int postId);
        //Task<string> GetFileNameAsync();

    }
}
