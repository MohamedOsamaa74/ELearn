using ELearn.Application.DTOs.MessageDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IMessageService
    {
        public Task<Response<ViewMessageDTO>> SendMessageAsync(SendMessageDTO Model);

        public Task<Response<List<ViewMessageDTO>>> GetMessagesByReceiverIdAsync(string ReceiverId);

        public Task<Response<ViewMessageDTO>> EditMessageAsync(int Id,SendMessageDTO Model);



    }
}
