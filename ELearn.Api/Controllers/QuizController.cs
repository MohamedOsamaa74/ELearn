using ELearn.Application.DTOs.QuizDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        public QuizController(IQuizService QuizService)
        {
            _quizService = QuizService;
        }
        [HttpPost("CreateNewQuiz")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDTO Model)
        {
            var response = await _quizService.CreateNewAsync(Model);
            return this.CreateResponse(response);

        }
    }
}
