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
        #region Fields and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }
        #endregion

        #region CreateNewAsync
        public async Task<Response<QuestionDTO>> CreateNewAsync(QuestionDTO Model, string Parent, int ParentId)
        {
            try
            {
                var question = _mapper.Map<Question>(Model);
                if (Parent == "Quiz")
                {
                    var quiz = await _unitOfWork.Quizziz.GetByIdAsync(ParentId);
                    if(quiz == null)
                        return ResponseHandler.BadRequest<QuestionDTO>("Quiz is not valid");
                    question.QuizId = ParentId;
                }
                else if (Parent == "Survey")
                {
                    var survey = await _unitOfWork.Surveys.GetByIdAsync(ParentId);
                    if (survey == null)
                        return ResponseHandler.BadRequest<QuestionDTO>("Survey is not valid");
                    question.SurveyId = ParentId;
                }
                else
                    return ResponseHandler.BadRequest<QuestionDTO>("Parent is not valid");

                await _unitOfWork.Questions.AddAsync(question);
                return ResponseHandler.Success(Model);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<QuestionDTO>(ex.Message);
            }
        }
        #endregion

        #region UpdateAsync
        public async Task<Response<QuestionDTO>> UpdateAsync(int Id, QuestionDTO Model)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(Id);
                if (question == null)
                    return ResponseHandler.NotFound<QuestionDTO>("Question not found");

                question = _mapper.Map(Model, question);
                await _unitOfWork.Questions.UpdateAsync(question);
                return ResponseHandler.Success(Model);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<QuestionDTO>(ex.Message);
            }
        }
        #endregion

        #region DeleteAsync
        public async Task<Response<QuestionDTO>> DeleteAsync(int Id)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(Id);
                if (question == null)
                    return ResponseHandler.NotFound<QuestionDTO>("Question not found");

                await _unitOfWork.Questions.DeleteAsync(question);
                return ResponseHandler.Deleted<QuestionDTO>();
            }
            catch (Exception ex)
        {
                return ResponseHandler.BadRequest<QuestionDTO>(ex.Message);
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<Response<QuestionDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(Id);
                if (question == null)
                    return ResponseHandler.NotFound<QuestionDTO>("Question not found");

                var questionDTO = _mapper.Map<QuestionDTO>(question);
                return ResponseHandler.Success(questionDTO);
        }
            catch (Exception ex)
        {
                return ResponseHandler.BadRequest<QuestionDTO>(ex.Message);
            }
        }
        #endregion

        #region RecieveStudentAnswerAsync
        public async Task<Response<QuestionAnswerDTO>> RecieveStudentAnswerAsync(QuestionAnswerDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                    return ResponseHandler.BadRequest<QuestionAnswerDTO>("User not found");
                var question = await _unitOfWork.Questions.GetByIdAsync(Model.QuestionId);
                if (question == null)
                    return ResponseHandler.NotFound<QuestionAnswerDTO>("Question not found");
                
                if(Model.Option != question.Option1 && Model.Option != question.Option2 && Model.Option != question.Option3 && Model.Option != question.Option4 && Model.Option!=question.Option5)
                    return ResponseHandler.BadRequest<QuestionAnswerDTO>("Answer is not valid");

                var userAnswer = _mapper.Map<UserAnswerQuestion>(Model);
                userAnswer.UserId = user.Id;
                await _unitOfWork.UserAnswerQuestions.AddAsync(userAnswer);
                if(question.CorrectOption != null && question.CorrectOption == Model.Option)
                     Model.Score = question.Grade;
                else Model.Score = 0;
                return ResponseHandler.Success(Model);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<QuestionAnswerDTO>(ex.Message);
            }
        }
        #endregion

        #region GetStudentAnswerAsync
        public async Task<Response<ICollection<QuestionAnswerDTO>>> GetStudentAnswersAsync(string Parent, int ParentId, string UserId)
        {
            try
            {
                ICollection<QuestionAnswerDTO> answers = new List<QuestionAnswerDTO>();
                if (Parent == "Quiz")
                {
                    var quiz = await _unitOfWork.Quizziz.GetByIdAsync(ParentId);
                    if (quiz == null)
                        return ResponseHandler.BadRequest<ICollection<QuestionAnswerDTO>>("Quiz is not valid");
                    var questions = await _unitOfWork.Questions.GetWhereAsync(q => q.QuizId == ParentId);
                    foreach (var question in questions)
                    {
                        var allAnswers = await _unitOfWork.UserAnswerQuestions.GetWhereAsync(q=>q.QuestionId == question.Id);
                        var answer = allAnswers.Where(q => q.UserId == UserId).ToList();
                        if (answer != null)
                            answers.Add(_mapper.Map<QuestionAnswerDTO>(answer));
                    }
                }
                else if (Parent == "Survey")
                {
                    var survey = await _unitOfWork.Surveys.GetByIdAsync(ParentId);
                    if (survey == null)
                        return ResponseHandler.BadRequest<ICollection<QuestionAnswerDTO>>("Survey is not valid");
                    var questions = await _unitOfWork.Questions.GetWhereAsync(s => s.SurveyId == ParentId);
                    foreach (var question in questions)
                    {
                        var allAnswers = await _unitOfWork.UserAnswerQuestions.GetWhereAsync(q => q.QuestionId == question.Id);
                        var answer = allAnswers.Where(q => q.UserId == UserId).SingleOrDefault();
                        if (answer != null)
                            answers.Add(_mapper.Map<QuestionAnswerDTO>(answer));
                    }
                }
                else
                    return ResponseHandler.BadRequest<ICollection<QuestionAnswerDTO>>("Parent is not valid");

                return ResponseHandler.Success(answers);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<QuestionAnswerDTO>>(ex.Message);
            }
        }
        #endregion
    }
}
