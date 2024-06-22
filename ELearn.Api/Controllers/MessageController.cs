using ELearn.Application.DTOs.MessageDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        #region Constructor
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        #endregion

        #region SendMessage
        [HttpPost("SendMessage")]
        [Authorize(Roles = "Admin ,Staff ,Student")]
        public async Task<IActionResult> SendMessage(SendMessageDTO Model)
        {
            var response = await _messageService.SendMessageAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetMessages
        [HttpGet("GetChatMessages/{ReceiverId}")]
        [Authorize(Roles = "Admin ,Staff ,Student")]
        public async Task<IActionResult> GetMessages([FromRoute]string ReceiverId)
        {
            var response = await _messageService.GetMessagesByReceiverIdAsync(ReceiverId);
            return this.CreateResponse(response);
        }
        #endregion

        #region EditMessage
        [HttpPut("EditMessage/{Id}")]
        [Authorize(Roles = "Admin ,Staff ,Student")]
        public async Task<IActionResult> EditMessage([FromRoute]int Id, SendMessageDTO Model)
        {
            var response = await _messageService.EditMessageAsync(Id, Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region DeleteMessage
        [HttpDelete("DeleteMessage/{Id}")]
        [Authorize(Roles = "Admin ,Staff ,Student")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int Id)
        {
            var response = await _messageService.DeleteMessageAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region DeleteAllMessages
        [HttpDelete("DeleteAllMessages/{ReceiverId}")]
        [Authorize(Roles = "Admin ,Staff ,Student")]
        public async Task<IActionResult> DeleteAllMessages([FromRoute] string ReceiverId)
        {
            var response = await _messageService.DeleteAllMessagesAsync(ReceiverId);
            return this.CreateResponse(response);
        }
        #endregion
    }
}
