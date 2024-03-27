using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(byte[] file, string folderName, string fileName);
        Task<byte[]> DownloadFileAsync(string folderName, string fileName);
        Task<bool> DeleteFileAsync(string folderName, string fileName);
    }
}
