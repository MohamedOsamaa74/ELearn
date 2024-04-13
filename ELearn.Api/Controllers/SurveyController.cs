using ELearn.Application.DTOs;
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
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        #region Create New
        [HttpPost("CreateSurvey")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Create([FromBody] SurveyDTO Model)
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
        [HttpGet("GetSurveysByCreator")]
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

        #region GetFromGroups
        [HttpGet("GetSurveysFromGroup/{GroupId:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetFromGroups(int GroupId)
        {
            var response = await _surveyService.GetFromGroups(GroupId);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetFromUserGroups
        [HttpGet("GetSurveysFromUserGroup/")]
        [Authorize]
        public async Task<IActionResult> GetFromUserGroups()
        {
            var response = await _surveyService.GetFromUserGroups();
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete One
        [HttpDelete("DeleteSurvey/{Id:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = await _surveyService.DeleteAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion
    }
}
