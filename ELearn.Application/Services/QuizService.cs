using AutoMapper;
using ELearn.Application.DTOs.GroupDTOs;
using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.DTOs.QuizDTOs;
using ELearn.Application.DTOs.SurveyDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<Response<CreateQuizDTO>> CreateNewAsync(CreateQuizDTO Model, int groupID)
        {
            try
            {
                var quiz = _mapper.Map<CreateQuizDTO, Quiz>(Model);
                quiz.UserId = _userService.GetCurrentUserAsync().Result.Id;
                if (quiz.UserId == null)
                {
                    return ResponseHandler.BadRequest<CreateQuizDTO>("User not found");
                }
                if (quiz.Start >= quiz.End || quiz.Start <= DateTime.Now)
                {
                    return ResponseHandler.BadRequest<CreateQuizDTO>("Start date must be less than end date, greater than current date and not equal to end date");
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
                        var question = _mapper.Map<QuestionDTO, Question>(createQuestionDTO);
                        if (question.CorrectOption == null || question.CorrectOption == "" || question.CorrectOption == string.Empty)
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
        public async Task<Response<EditQuizDTO>> UpdateQuizAsync(EditQuizDTO Model, int QuizId)
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
                if (Model.Start >= Model.End || Model.Start <= DateTime.Now)
                {
                    return ResponseHandler.BadRequest<EditQuizDTO>("Start date must be less than end date, greater than current date and not equal to end date");
                }
               
                var t= _questionService.GetQuestionsByQuizIdAsync(QuizId);
                var tt = t.Result.Data.Sum(q => q.Grade);
                //var totalQuestionGrade = oldquiz.Questions.Sum(q => q.Grade);
                if (tt != Model.Grade )
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

        #region Get Quiz By ID

        public async Task<Response<ViewQuizDTO>> GetQuizByIdAsync(int quizId)
        {
            try
            {
                var quiz = await _unitOfWork.Quizziz.GetByIdAsync(quizId);
                if (quiz == null)
                {
                    return ResponseHandler.NotFound<ViewQuizDTO>();
                }
                var quizDto = _mapper.Map<ViewQuizDTO>(quiz);
                var questions = await _questionService.GetQuestionsByQuizIdAsync(quiz.Id);
                quizDto.Questions = questions.Data;
                return ResponseHandler.Success(quizDto);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewQuizDTO>($"An error occurred while retrieving Quiz: {ex.Message}");
            }
        }
        #endregion

        #region Get All Quizes

        public async Task<Response<ICollection<ViewQuizDTO>>> GetAllQuizzesAsync()
        {
            try
            {
                var quizzes = await _unitOfWork.Quizziz.GetAllAsync();
                if (quizzes.IsNullOrEmpty())
                {
                    return ResponseHandler.NotFound<ICollection<ViewQuizDTO>>();
                }
                ICollection<ViewQuizDTO> quizDtos = new List<ViewQuizDTO>();
                foreach (var quiz in quizzes)
                {
                    var quizDto = _mapper.Map<ViewQuizDTO>(quiz);
                    var questions = await _questionService.GetQuestionsByQuizIdAsync(quiz.Id);
                    quizDto.Questions = questions.Data;
                    quizDtos.Add(quizDto);
                }

                return ResponseHandler.Success(quizDtos);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewQuizDTO>>($"An error occurred while retrieving Quizzes: {ex.Message}");
            }
        }
        #endregion

        #region Delete
        public async Task<Response<CreateQuizDTO>> DeleteAsync(int Id)
        {
            try
            {
                var quiz = await _unitOfWork.Quizziz.GetByIdAsync(Id);
                if (quiz == null)
                    return ResponseHandler.NotFound<CreateQuizDTO>("Quiz not found");

                await _unitOfWork.Quizziz.DeleteAsync(quiz);
                return ResponseHandler.Deleted<CreateQuizDTO>();
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<CreateQuizDTO>(ex.Message);
            }
        }
        #endregion

        #region ReceiveStudentQuizResponseAsync
        //  useranswerquizهنا بحسب الاسكور بتاع كل طال وبخزنه في 

        public async Task<Response<QuizResultDTO>> ReceiveStudentQuizResponsesAsync(UserAnswerQuizDTO userAnswerDto)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user is null)
                    return ResponseHandler.NotFound<QuizResultDTO>("There is no such User");
                var quiz = await _unitOfWork.Quizziz.GetByIdAsync(userAnswerDto.QuizId);
                if (quiz is null)
                    return ResponseHandler.NotFound<QuizResultDTO>("There is no such Quiz");

        
                var totalScore = 0;
                foreach (var answer in userAnswerDto.Answers)
                {
                    var question = await _unitOfWork.Questions.GetByIdAsync(answer.QuestionId);
                    if (question is null || question.QuizId != userAnswerDto.QuizId )
                        return ResponseHandler.NotFound<QuizResultDTO>("There is no such Question in this Quiz");
                    var recieveAnswer = await _questionService.RecieveStudentAnswerAsync(answer);
                    if (!recieveAnswer.Succeeded)
                        return ResponseHandler.BadRequest<QuizResultDTO>($"An Error Occurred, {recieveAnswer.Message}");
                    totalScore += recieveAnswer.Data.Score ??0;
                }
                 var userAnswerQuiz = new UserAnswerQuiz { QuizId =userAnswerDto.QuizId , UserId = user.Id ,Grade=totalScore};
                await _unitOfWork.UserAnswerQuizziz.AddAsync(userAnswerQuiz);
                var quizResult = new QuizResultDTO()
                {
                    QuizId = userAnswerDto.QuizId,
                    StudentName = user.UserName,
                    TotalScore = totalScore,
                    
                };

                return ResponseHandler.Success(quizResult);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<QuizResultDTO>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        #region Get All Responses
        public async Task<Response<List<QuizResultDTO>>> GetAllQuizResponsesAsync(int quizId)
        {
            try
            {

                var userResponses = await _unitOfWork.UserAnswerQuizziz
                    .GetWhereAsync(q => q.QuizId == quizId);

                if (userResponses is null || !userResponses.Any())
                    return ResponseHandler.NotFound<List<QuizResultDTO>>("No responses found for this quiz");

                var quizResults = new List<QuizResultDTO>();

                foreach (var userResponse in userResponses)
                {
                  
                    var user = await _userService.GetCurrentUserAsync();
                    if (user is null)
                        continue; // Skip if user not found

                    var quizResult = new QuizResultDTO
                    {
                        QuizId = quizId,
                        StudentName = user.UserName,
                        TotalScore = userResponse.Grade 
                    };

                    quizResults.Add(quizResult);
                }

                return ResponseHandler.Success(quizResults);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<List<QuizResultDTO>>($"An error occurred: {ex.Message}");
            }
        }

        #endregion

        #region GetStudentResponses
        // وخزنت اجابة الطالب GetStudentAnswersAsyncهنا خزنة الاجابة واستخدمت 
        //المفروض هنا برجع اجابات طالب في الكويز
        //get student response by id
        public async Task<Response<UserAnswerQuizDTO>> GetUserAnswerAsync(int quizId, string UserId)
        {
            try
            {
                var user = await _userService.GetByIdAsync(UserId);
                if (user is null)
                    return ResponseHandler.NotFound<UserAnswerQuizDTO>("There is no such User");
                var quiz = await _unitOfWork.Quizziz.GetByIdAsync(quizId);
                if (quiz is null)
                    return ResponseHandler.NotFound<UserAnswerQuizDTO>("There is no such Survey");
                var studentAnswers = await _questionService.GetStudentAnswersAsync("Quiz", quizId, UserId);
                var studentAnswer = _mapper.Map<ICollection<QuestionAnswerDTO>, ICollection<QuestionQuizDTO>>(studentAnswers.Data);
                var userAnswers = new UserAnswerQuizDTO()
                {
                    QuizId = quizId,
                    Answers = studentAnswer
                };
                return ResponseHandler.Success(userAnswers);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<UserAnswerQuizDTO>($"An Error Occorred, {Ex}");
            }
        }
        #endregion
    }
}