using AutoMapper;
using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.DTOs.MessageDTOs;
using ELearn.Application.DTOs.PostDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.InfraStructure.Validations;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class MessageService : IMessageService
    {
        #region Constructor
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public MessageService(AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
            _fileService = fileService;
        }
        #endregion

        #region SendMessage
        public async Task<Response<ViewMessageDTO>> SendMessageAsync(SendMessageDTO Model)
        {
            try
            {
                var sender = await _userService.GetCurrentUserAsync();
                var Receiver = await _unitOfWork.Users.GetByIdAsync(Model.ReceiverId);
                if (sender == null)
                {
                    return ResponseHandler.BadRequest<ViewMessageDTO>("Sender not found");
                }

                if (Receiver == null || string.IsNullOrEmpty(Receiver.PublicKey))
                {
                    return ResponseHandler.BadRequest<ViewMessageDTO>("Receiver not found or public key missing");
                }
                var encryptedText = Encrypt(Model.Text, Receiver.PublicKey);
                var newMessage = new Message
                {
                    SenderId = sender.Id,
                    ReceiverId = Model.ReceiverId,
                    Text = encryptedText,
                    CreationDate = DateTime.Now
                };
                // Validate the newMessage object
                var validate = new MessageValidation().Validate(newMessage);
                if (!validate.IsValid)
                {
                    // Get the errors 
                    var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
                    return ResponseHandler.BadRequest<ViewMessageDTO>(null,errors);
                }
                await _unitOfWork.Messages.AddAsync(newMessage);
                var ViewMessage = _mapper.Map<ViewMessageDTO>(newMessage);
                var deT = Decrypt(newMessage.Text, Receiver.PrivateKey);
                ViewMessage.Text = deT;
                string ViewUrl;
                if (Model.File != null)
                {
                    var uploadFileDto = new UploadFileDTO()
                    { File = Model.File, FolderName = "Messages", ParentId = newMessage.Id };
                    var newFile = await _fileService.UploadFileAsync(uploadFileDto);
                    ViewMessage.url = newFile.Data.ViewUrl;

                }

                return ResponseHandler.Created(ViewMessage);

            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewMessageDTO>(ex.Message);
            }
        }
        #endregion

        #region GetMessagesByUserId
        public async Task<Response<List<ViewMessageDTO>>> GetMessagesByReceiverIdAsync(string ReceiverId)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var Receiver = await _unitOfWork.Users.GetByIdAsync(ReceiverId);
                if (ReceiverId == null)
                {
                    return ResponseHandler.NotFound<List<ViewMessageDTO>>("Chat Not Found");
                }
                var messages = await _unitOfWork.Messages.GetWhereAsync(m =>
                ((m.ReceiverId == ReceiverId && m.SenderId == user.Id) || (m.ReceiverId == user.Id && m.SenderId == ReceiverId)) &&
                ((m.SenderId == user.Id && !m.IsDeletedBySender) || (m.ReceiverId == user.Id && !m.IsDeletedByReceiver)));

                var ViewMessages = _mapper.Map<List<ViewMessageDTO>>(messages);
                foreach (var viewMessage in ViewMessages)
                {
                    var id = viewMessage.Id;
                    var m = _unitOfWork.Messages.GetWhereSingleAsync(x => x.Id == id).Result;
                    var file = await _unitOfWork.Files.GetWhereSingleAsync(f => f.MessageId == id);
                    if (file != null)
                    {
                        viewMessage.url = file.ViewUrl.ToString();
                    }
                    if (!string.IsNullOrEmpty(viewMessage.Text) && !string.IsNullOrEmpty(Receiver.PrivateKey))
                    {
                        try
                        {
                            if(m.SenderId == user.Id)
                            {
                                var deT = Decrypt(m.Text, Receiver.PrivateKey);
                                viewMessage.Text = deT;
                            }
                            else
                            {
                                var deT = Decrypt(m.Text, user.PrivateKey);
                                viewMessage.Text = deT;
                            }
                        }
                        catch (Exception ex)
                        {
                            viewMessage.Text = "Error decrypting message";
                        }
                    }
                }

                return ResponseHandler.Success(ViewMessages);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<List<ViewMessageDTO>>(ex.Message);     
            }
        }
        #endregion

        #region EditMessage
        public async Task<Response<ViewMessageDTO>> EditMessageAsync(int Id, SendMessageDTO Model)
        {
            try
            {
                var message = await _unitOfWork.Messages.GetByIdAsync(Id);
                var Receiver = await _unitOfWork.Users.GetByIdAsync(message.ReceiverId);
                if (message == null)
                {
                    return ResponseHandler.NotFound<ViewMessageDTO>("Message not found");
                }
                var sender = await _userService.GetCurrentUserAsync();
                if (message.SenderId != sender.Id)
                {
                    return ResponseHandler.Unauthorized<ViewMessageDTO>("You are not allowed to edit this message");
                }
                var encryptedText = Encrypt(Model.Text, Receiver.PublicKey);
                message.Text = encryptedText;
                message.CreationDate= DateTime.UtcNow;
                await _unitOfWork.Messages.UpdateAsync(message);
                var ViewMessage = _mapper.Map<ViewMessageDTO>(message);
                var deT = Decrypt(message.Text, Receiver.PrivateKey);
                ViewMessage.Text = deT;
                string ViewUrl;
                //delete old file
                var file = await _unitOfWork.Files.GetWhereAsync(f => f.MessageId == Id);
                if (file != null)
                {
                    foreach (var f in file)
                    {
                        await _fileService.DeleteAsync(f.Id);
                    }
                }
                //add new file
                if (Model.File != null)
                {
                    var uploadFileDto = new UploadFileDTO()
                    { File = Model.File, FolderName = "Messages", ParentId = message.Id };
                    var newFile = await _fileService.UploadFileAsync(uploadFileDto);
                    ViewMessage.url = newFile.Data.ViewUrl;
                }
                return ResponseHandler.Success(ViewMessage);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewMessageDTO>(ex.Message);
            }
        }
        #endregion

        #region DeleteMessageFromTwoUsers
        public async Task<Response<ViewMessageDTO>> DeleteMessageAsync(int Id)
        {
            try
            {
                var message = await _unitOfWork.Messages.GetByIdAsync(Id);
                if (message == null)
                {
                    return ResponseHandler.NotFound<ViewMessageDTO>("Message not found");
                }
                var sender = await _userService.GetCurrentUserAsync();
                if (message.SenderId != sender.Id)
                {
                    return ResponseHandler.Unauthorized<ViewMessageDTO>("You are not allowed to delete this message");
                }
                await _unitOfWork.Messages.DeleteAsync(message);
                return ResponseHandler.Deleted<ViewMessageDTO>();
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewMessageDTO>(ex.Message);
            }
        }
        #endregion

        #region DeleteMessageFromOneUser
        public async Task<Response<ViewMessageDTO>> DeleteMessageFromOneUserAsync(int Id)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var message = await _unitOfWork.Messages.GetByIdAsync(Id);
                if (message == null)
                {
                    return ResponseHandler.NotFound<ViewMessageDTO>("Message not found");
                }
                var sender = await _userService.GetCurrentUserAsync();
                if (message.SenderId != sender.Id)
                {
                    return ResponseHandler.Unauthorized<ViewMessageDTO>("You are not allowed to delete this message");
                }
                if (message.SenderId == user.Id)
                {
                    message.IsDeletedBySender = true; // Flag the message as deleted by sender
                }
                else if (message.ReceiverId == user.Id)
                {
                    message.IsDeletedByReceiver = true; // Flag the message as deleted by receiver
                }
                await _unitOfWork.Messages.UpdateAsync(message);
                var viewMessage = _mapper.Map<ViewMessageDTO>(message);
                var Receiver = await _unitOfWork.Users.GetByIdAsync(message.ReceiverId);
                var deT = Decrypt(message.Text, Receiver.PrivateKey);
                viewMessage.Text = deT;
                var file = await _unitOfWork.Files.GetWhereSingleAsync(f => f.MessageId == Id);
                if (file != null)
                {
                    viewMessage.url = file.ViewUrl.ToString();
                }

                return ResponseHandler.Deleted<ViewMessageDTO>();
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewMessageDTO>(ex.Message);
            }
        }
        #endregion

        //#region DeleteAllMessagesFromTwoUsers
        //public async Task<Response<bool>> DeleteAllMessagesAsync(string ReceiverId)
        //{
        //    try
        //    {
        //        var user = await _userService.GetCurrentUserAsync();
        //        var Receiver = await _unitOfWork.Users.GetByIdAsync(ReceiverId);
        //        if (ReceiverId == null)
        //        {
        //            return ResponseHandler.NotFound<bool>("Chat Not Found");
        //        }
        //        var messages = await _unitOfWork.Messages.GetWhereAsync(m =>
        //                                   (m.ReceiverId == ReceiverId && m.SenderId == user.Id) ||
        //                                                              (m.ReceiverId == user.Id && m.SenderId == ReceiverId));
        //        foreach (var message in messages)
        //        {
        //            await _unitOfWork.Messages.DeleteAsync(message);
        //        }
        //        return ResponseHandler.Success(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseHandler.BadRequest<bool>(ex.Message);
        //    }
        //}
        //#endregion

        #region DeleteAllMessagesfromOneUser

        public async Task<Response<bool>> DeleteAllMessagesAsync(string ReceiverId)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var receiver = await _unitOfWork.Users.GetByIdAsync(ReceiverId);
                if (receiver == null)
                {
                    return ResponseHandler.NotFound<bool>("Chat Not Found");
                }

                var messages = await _unitOfWork.Messages.GetWhereAsync(m =>
                    (m.ReceiverId == ReceiverId && m.SenderId == user.Id) ||
                    (m.ReceiverId == user.Id && m.SenderId == ReceiverId));
                foreach (var message in messages)
                {
                    if (message.SenderId == user.Id)
                    {
                        message.IsDeletedBySender = true; // Flag the message as deleted by sender
                    }
                    else if (message.ReceiverId == user.Id)
                    {
                        message.IsDeletedByReceiver = true; // Flag the message as deleted by receiver
                    }
                    await _unitOfWork.Messages.UpdateAsync(message);
                }
 
                await _unitOfWork.Messages.SaveChangesAsync();

                return ResponseHandler.Success(true);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<bool>(ex.Message);
            }
        }
        #endregion










        #region Encryption
        private static string Encrypt(string plaintext, string publicKey)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                    var encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.OaepSHA1);
                    return Convert.ToBase64String(encryptedBytes).ToString();
                }
            }

            private static string Decrypt(string ciphertext, string privateKey)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                    var decryptedBytes = rsa.Decrypt(Convert.FromBase64String(ciphertext), RSAEncryptionPadding.OaepSHA1);
                    return Encoding.UTF8.GetString(decryptedBytes).ToString();
                }
            }
        
        #endregion


    }
}
