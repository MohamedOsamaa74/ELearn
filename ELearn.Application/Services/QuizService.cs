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
    public class QuizService : IQuizService
    {
        #region Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public QuizService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }
        #endregion

        #region CreateNewAsync
        public async Task<Response<CreateQuizDTO>> CreateNewAsync(CreateQuizDTO Model)
        {
            try
            {
                var quiz = _mapper.Map<CreateQuizDTO, Quiz>(Model);
                quiz.UserId = _userService.GetCurrentUserAsync().Result.Id;
                quiz.GroupId = 2;

                if (Model.Questions != null && Model.Questions.Any())
                {
                    var questions = new List<Question>();
                    foreach (var createQuestionDTO in Model.Questions)
                    {
                        var question = _mapper.Map<CreateQuestionDTO, Question>(createQuestionDTO);
                        question.Quiz = quiz; 
                        questions.Add(question);
                    }
                    quiz.Questions = questions; 
                }

                await _unitOfWork.Quizziz.AddAsync(quiz);
                return ResponseHandler.Created(Model);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<CreateQuizDTO>(ex.Message);
            }
        }
        #endregion
    }
}
