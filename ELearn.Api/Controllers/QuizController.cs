using ELearn.Application.DTOs.QuizDTOs;
using ELearn.Application.DTOs.SurveyDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        #region Fields & Constructor
        private readonly IQuizService _quizService;
        public QuizController(IQuizService QuizService)
        {
            _quizService = QuizService;
        }
        #endregion

        #region CreateNewQuiz 
        [HttpPost("CreateNewQuiz")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDTO Model, [FromQuery] int groupID)
        {
            var response = await _quizService.CreateNewAsync(Model, groupID);
            return this.CreateResponse(response);

        }
        #endregion

        #region UpdateQuiz
        [HttpPut("UpdateQuiz")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> UpdateQuiz([FromBody] EditQuizDTO Model, [FromQuery] int quizID)
        {
            var response = await _quizService.UpdateQuizAsync(Model, quizID);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get Quiz By ID
        [HttpGet("GetById/{QuizId:int}")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> GeQuizById(int QuizId)
        {
            var response = await _quizService.GetQuizByIdAsync(QuizId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get All Quizes
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _quizService.GetAllQuizzesAsync();
            return this.CreateResponse(response);
        }
        #endregion

        #region Get Quizzes From Group
        [HttpPost("GetQuizzesFromGroup")]
        [Authorize]
        public async Task<IActionResult> GetQuizzesFromGroup([FromQuery] int groupId)
        {
            if(User.IsInRole("Admin"))
                RedirectToAction("GetAll");
            var response = await _quizService.GetAllQuizzesFromGroupAsync(groupId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete Quiz
        [HttpDelete("Delete/{QuizId:int}")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> DeleteQuiz(int QuizId)
        {
            var response = await _quizService.DeleteAsync(QuizId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Receive Student Quiz Response
        [HttpPost("SubmitResponse")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitResponseAsync([FromBody] UserAnswerQuizDTO userAnswerDTO)
        {
            var response = await _quizService.SubmitResponsesAsync(userAnswerDTO);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetUserResponse
        [HttpGet("GetUserAnswers/{QuizId:int}/{UserId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetUserAnswersAsync(int QuizId, string UserId)
        {
            var response = await _quizService.GetUserAnswerAsync(QuizId, UserId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get All Responses
        [HttpGet("GetAllResponses/{QuizId:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetAllQuizResponsesAsync(int QuizId)
        {
            var response = await _quizService.GetAllQuizResponsesAsync(QuizId);
            return this.CreateResponse(response);
        }
        #endregion

    }
}