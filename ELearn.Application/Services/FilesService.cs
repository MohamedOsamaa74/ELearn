using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class FilesService : IFileService
    {
        public async Task<FileEntity> UploadFileAsync(byte[] file, string folderName, string fileName)
        {
            throw new NotImplementedException();
        }



        public Task<byte[]> DownloadFileAsync(string folderName, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFileAsync(string folderName, string fileName)
        {
            throw new NotImplementedException();
        }

        Task<string> IFileService.UploadFileAsync(byte[] file, string folderName, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
