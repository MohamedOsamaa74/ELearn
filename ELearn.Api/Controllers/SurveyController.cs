using ELearn.Application.DTOs;
using ELearn.Application.DTOs.SurveyDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        #region Fields and Constructor
        private readonly ISurveyService _surveyService;
        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }
        #endregion

        #region Create New
        [HttpPost("CreateSurvey")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Create([FromBody] CreateSurveyDTO Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _surveyService.CreateNewAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get By Id
        [HttpGet("GetSurvey/{Id:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetById(int Id)
        {
            var response = await _surveyService.GetByIdAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetSurveyofCurrentUser
        [HttpGet("GetByCreator")]
        public async Task<IActionResult> GetSurveysByCreator()
        {
            var response = await _surveyService.GetSurveysByCreator();
            return this.CreateResponse(response);
        }

        #endregion

        #region GetAll
        [HttpGet("GetAllSurveys")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _surveyService.GetAllAsync();
            return this.CreateResponse(response);
        }
        #endregion

        #region GetFromGroup
        [HttpGet("GetSurveysFromGroup/{GroupId:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetFromGroups(int GroupId)
        {
            var response = await _surveyService.GetFromGroup(GroupId);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetFromUserGroups
        [HttpGet("GetFromUserGroups")]
        [Authorize]
        public async Task<IActionResult> GetFromUserGroups()
        {
            var response = await _surveyService.GetFromUserGroups();
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete One
        [HttpDelete("Delete/{Id:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            var response = await _surveyService.DeleteAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete Many
        [HttpDelete("DeleteMany")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteManyAsync([FromBody] int[] Ids)
        {
            var response = await _surveyService.DeleteManyAsync(Ids);
            return this.CreateResponse(response);
        }
        #endregion

        #region Submit Response
        [HttpPost("SubmitResponse")]
        [Authorize]
        public async Task<IActionResult> SubnitResponseAsync([FromBody] UserAnswerSurveyDTO Model)
        {
            var response = await _surveyService.SubmitResponseAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetUserAnswers
        [HttpGet("GetUserAnswers/{SurveyId:int}/{UserId}")]
        [Authorize(Roles ="Admin, Staff")]
        public async Task<IActionResult> GetUserAnswersAsync(int SurveyId, string UserId)
        {
            var response = await _surveyService.GetUserAnswerAsync(SurveyId, UserId);
            return this.CreateResponse(response);
        }
        #endregion
    }
}