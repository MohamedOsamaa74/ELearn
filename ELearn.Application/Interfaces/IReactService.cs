using ELearn.Application.DTOs.ReactDTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IReactService
    {
        public Task<Response<ReactDTO>> CreateNewAsync(ReactDTO reactDTO);
        public Task<Response<string>> DeleteAsync(int Id);
        public Task<Response<ICollection<string>>> GetReactorsAsync(ReactDTO reactDTO);
        //GetByReactor (هل محتاجينه عندنا؟)
    }
}