using ELearn.Application.DTOs.ReactDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReactController : ControllerBase
    {
        #region Fields
        private readonly IReactService _reactService;
        public ReactController(IReactService reactService)
        {
            _reactService = reactService;
        }
        #endregion

        #region Create New
        [HttpPost("CreateReact")]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult> CreateReactAsync(ReactDTO reactDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _reactService.CreateNewAsync(reactDTO);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete
        [HttpDelete("/DeleteReact{Id:int}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult>DeleteReact(int Id)
        {
            var response = await _reactService.DeleteAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetReactors
        [HttpGet("/GetReactors")]
        [Authorize(Roles = "Student, Admin")]
        public async Task<IActionResult>GetReactorsAsync(ReactDTO reactDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _reactService.GetReactorsAsync(reactDTO);
            return this.CreateResponse(response);
        }
        #endregion
    }
}
