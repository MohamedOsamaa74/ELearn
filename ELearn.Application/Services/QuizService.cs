using AutoMapper;
using ELearn.Application.DTOs.GroupDTOs;
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
        private readonly IQuestionService _questionService;
        public QuizService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IQuestionService questionService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
            _questionService = questionService;
        }
        #endregion

        #region CreateNewAsync
        public async Task<Response<CreateQuizDTO>> CreateNewAsync(CreateQuizDTO Model,int groupID)
        {
            try
            {
                var quiz = _mapper.Map<CreateQuizDTO, Quiz>(Model);
                quiz.UserId = _userService.GetCurrentUserAsync().Result.Id;
                if(quiz.UserId == null)
                {
                    return ResponseHandler.BadRequest<CreateQuizDTO>("User not found");
                }
                if(quiz.Start > quiz.End || quiz.Start < DateTime.Now)
                {
                    return ResponseHandler.BadRequest<CreateQuizDTO>("Start date must be less than end date And greater than current date");
                }
                var group = await _unitOfWork.Groups.GetByIdAsync(groupID);
                if (group is null)
                {
                    return ResponseHandler.BadRequest<CreateQuizDTO>("The Group Doesn't Exist");
                }
                quiz.GroupId = groupID;
                if (Model.Questions != null && Model.Questions.Any())
                {
                    var totalQuestionGrade = Model.Questions.Sum(q => q.Grade);
                    if (totalQuestionGrade != quiz.Grade)
                    {
                        return ResponseHandler.BadRequest<CreateQuizDTO>("Sum of question grades doesn't match quiz grade");
                    }
                    var questions = new List<Question>();
                    foreach (var createQuestionDTO in Model.Questions)
                    {
                        var question = _mapper.Map<CreateQuestionDTO, Question>(createQuestionDTO);
                        if (question.CorrectOption == null || question.CorrectOption == "")
                            return ResponseHandler.BadRequest<CreateQuizDTO>("Correct Option is required");
                        if (question.CorrectOption != question.Option1 && question.CorrectOption != question.Option2 && question.CorrectOption != question.Option3 && question.CorrectOption != question.Option4 && question.CorrectOption != question.Option5)
                            return ResponseHandler.BadRequest<CreateQuizDTO>("Invalid Correct Option");
                        question.QuizId = quiz.Id;
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

        #region UpdateQuiz
        public async Task<Response<EditQuizDTO>> UpdateQuiz(EditQuizDTO Model , int QuizId)
        {
            try
            {
                var oldquiz = await _unitOfWork.Quizziz.GetByIdAsync(QuizId);
                if (oldquiz is null)
                {
                    return ResponseHandler.NotFound<EditQuizDTO>("Quiz not found");
                }
                if (oldquiz.UserId != _userService.GetCurrentUserAsync().Result.Id)
                {
                    return ResponseHandler.Unauthorized<EditQuizDTO>();
                }
                if (Model.Start > Model.End || Model.Start < DateTime.Now)
                {
                    return ResponseHandler.BadRequest<EditQuizDTO>("Start date must be less than end date And greater than current date");
                }
                var totalQuestionGrade = oldquiz.Questions.Sum(q => q.Grade);
                if (totalQuestionGrade != Model.Grade)
                {
                    return ResponseHandler.BadRequest<EditQuizDTO>("Sum of question grades doesn't match quiz grade");
                }
                oldquiz.title = Model.title;
                oldquiz.Start = Model.Start;
                oldquiz.End = Model.End;
                oldquiz.Grade = Model.Grade;

                #region Old
                //if (Model.Questions != null && Model.Questions.Any())
                //{
                //    var totalQuestionGrade = Model.Questions.Sum(q => q.Grade);
                //    if (totalQuestionGrade != Model.Grade)
                //    {
                //        return ResponseHandler.BadRequest<CreateQuizDTO>("Sum of question grades doesn't match quiz grade");
                //    }
                //    var questions = new List<Question>();
                //    foreach (var createQuestionDTO in Model.Questions)
                //    {
                //        var question = _mapper.Map<CreateQuestionDTO, Question>(createQuestionDTO);
                //        if (question.CorrectOption == null || question.CorrectOption == "" || question.CorrectOption == string.Empty)
                //            return ResponseHandler.BadRequest<CreateQuizDTO>("Correct Option is required");
                //        if (question.CorrectOption != question.Option1 && question.CorrectOption != question.Option2 && question.CorrectOption != question.Option3 && question.CorrectOption != question.Option4 && question.CorrectOption != question.Option5)
                //            return ResponseHandler.BadRequest<CreateQuizDTO>("Invalid Correct Option");
                //        question.Quiz = oldquiz;
                //        questions.Add(question);
                //    }
                //    oldquiz.Questions = questions;
                //}

                //oldquiz.IsPublished = Model.IsPublished;
                //oldquiz.IsClosed = Model.IsClosed;
                //oldquiz.IsDeleted = Model.IsDeleted;
                //oldquiz.UpdatedAt = DateTime.Now; 
                #endregion

                await _unitOfWork.Quizziz.UpdateAsync(oldquiz);
                return ResponseHandler.Updated(Model);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<EditQuizDTO>(ex.Message);
            }
        }
        #endregion


    }
}
