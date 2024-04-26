using AutoMapper;
using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.DTOs.QuizDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class QuestionService : IQuestionService
    {

        #region Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public QuestionService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }
        #endregion
        public async Task<Response<CreateQuestionDTO>> CreateQuestion(CreateQuestionDTO Model)
        {

            //var question = _mapper.Map<CreateQuestionDTO, Question>(Model);
            //if (question.CorrectOption == null || question.CorrectOption == "")
            //    return ResponseHandler.BadRequest<CreateQuestionDTO>("Correct Option is required");
            //if (question.CorrectOption != question.Option1 && question.CorrectOption != question.Option2 && question.CorrectOption != question.Option3 && question.CorrectOption != question.Option4 && question.CorrectOption != question.Option5)
            //    return ResponseHandler.BadRequest<CreateQuestionDTO>("Invalid Correct Option");

            //await _unitOfWork.Questions.AddAsync(question);
            return ResponseHandler.Created<CreateQuestionDTO>(Model);
        }
        public Task<Response<CreateQuestionDTO>> UpdateQuestion(CreateQuestionDTO Model, int questionID)
        {
           return null;
        }
    }
}
