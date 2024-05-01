using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.DTOs.SurveyDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;

namespace ELearn.Application.Services
{
    public class SurveyService : ISurveyService
    {
        #region Fields & Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;
        public SurveyService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IQuestionService questionService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
            _questionService = questionService;
        }
        #endregion

        #region CreateNew
        public async Task<Response<CreateSurveyDTO>> CreateNewAsync(CreateSurveyDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var survey = _mapper.Map<Survey>(Model);
                survey.CreatorId = user.Id;
                await _unitOfWork.Surveys.AddAsync(survey);
                
                var result = await SendToGroups(survey.Id, Model.GroupIds);
                foreach (var question in Model.Questions)
                {
                    await _questionService.CreateNewAsync(question, "Survey", survey.Id);
                }
                return ResponseHandler.Success(Model);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<CreateSurveyDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetById
        public async Task<Response<CreateSurveyDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var survey = await _unitOfWork.Surveys.GetByIdAsync(Id);
                if (survey is null)
                    return ResponseHandler.NotFound<CreateSurveyDTO>("There is no such Survey");
                var surveyDto = _mapper.Map<CreateSurveyDTO>(survey);
                surveyDto.GroupIds = await _unitOfWork.GroupSurveys
                    .GetWhereSelectAsync(v => v.SurveyId == Id, v => v.GroupId);
                var Questions = await _unitOfWork.Questions.GetWhereAsync(q => q.SurveyId == Id);
                surveyDto.Questions = _mapper.Map<ICollection<QuestionDTO>>(Questions);
                return ResponseHandler.Success(surveyDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<CreateSurveyDTO>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        public Task<Response<CreateSurveyDTO>> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ICollection<CreateSurveyDTO>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }


        public Task<Response<ICollection<CreateSurveyDTO>>> GetFromGroups(int GroupId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ICollection<CreateSurveyDTO>>> GetFromUserGroups()
        {
            throw new NotImplementedException();
        }

        public Task<Response<ICollection<CreateSurveyDTO>>> GetSurveysByCreator()
        {
            throw new NotImplementedException();
        }

        #region Private Methods
        private async Task<string> SendToGroups(int surveyId, ICollection<int>Groups)
        {
            try
            {
                foreach (var groupId in Groups)
                {
                    if (await _unitOfWork.Groups.GetByIdAsync(groupId) == null)
                        return "Invalid Group Id";

                    var groupVoting = await _unitOfWork.GroupSurveys
                        .GetWhereAsync(g => g.GroupId == groupId && g.SurveyId == surveyId);

                    if (!groupVoting.IsNullOrEmpty())
                        continue;
                    GroupSurvey NewGroupSurvey = new GroupSurvey()
                    {
                        GroupId = groupId,
                        SurveyId = surveyId
                    };
                    await _unitOfWork.GroupSurveys.AddAsync(NewGroupSurvey);
                }
                return "Success";
            }
            catch(Exception Ex)
            {
                return $"An Error Occurred, {Ex}";
            }
        }
        #endregion

        /*#region Create
        public async Task<Response<SurveyDTO>> CreateNewAsync(SurveyDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var survey = _mapper.Map<Survey>(Model);
                survey.ApplicationUserId = user.Id;
                await _unitOfWork.Surveys.AddAsync(survey);
                if (Model.Options.IsNullOrEmpty() || Model.Options.Count() < 2)
                    return ResponseHandler.BadRequest<SurveyDTO>("You have to insert at least two options");
                foreach (var text in Model.Options)
                {
                    Option option = new Option() { Text = text, SurveyId = survey.Id };
                    await _unitOfWork.Options.AddAsync(option);
                }
                await SendToGroupsAsync(Model.groups, survey.Id);
                return ResponseHandler.Success(Model);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<SurveyDTO>($"An Error Occurred, {Ex}");
            }
        }

        #endregion

        #region GetById
        public async Task<Response<SurveyDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var survey = await _unitOfWork.Surveys.GetByIdAsync(Id);
                if (survey is null)
                    return ResponseHandler.NotFound<SurveyDTO>("There is no such Survey");
                var surveyDto = _mapper.Map<SurveyDTO>(survey);
                surveyDto.groups = await _unitOfWork.GroupSurveys
                    .GetWhereSelectAsync(v => v.Id == Id, v => v.GroupId);
                surveyDto.Options = await _unitOfWork.Options.GetWhereSelectAsync(opt => opt.SurveyId == Id, opt => opt.Text);
                return ResponseHandler.Success(surveyDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<SurveyDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetByCreator
        public async Task<Response<ICollection<SurveyDTO>>> GetSurveysByCreator()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var surveys = await _unitOfWork.Surveys.GetWhereSelectAsync(v => v.ApplicationUserId == user.Id, v => v.Id);

                if (surveys is null)
                    return ResponseHandler.NotFound<ICollection<SurveyDTO>>("There are No Votings yet");
                ICollection<SurveyDTO> surveysDto = new List<SurveyDTO>();
                foreach (var survey in surveys)
                {
                    var surveydto = await GetByIdAsync(survey);
                    surveysDto.Add(surveydto.Data);
                }
                return ResponseHandler.Success(surveysDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<SurveyDTO>>($"An Error Occurred, {Ex}");
            }


        }

        #endregion

        #region GetFromUserGroups
        public async Task<Response<ICollection<SurveyDTO>>> GetFromUserGroups()
        {
            try
            {

                var user = await _userService.GetCurrentUserAsync();
                var groups = await _unitOfWork.UserGroups.GetWhereSelectAsync(u => u.UserId == user.Id, g => g.GroupId);
                var surveys = new List<int>();
                foreach (var group in groups)
                {
                    surveys.AddRange(await _unitOfWork.GroupSurveys.
                    GetWhereSelectAsync(v => v.GroupId == group, v => v.SurveyId));
                }
                ICollection<SurveyDTO> surveysDto = new List<SurveyDTO>();
                foreach (var survey in surveys)
                {
                    var surveydto = await GetByIdAsync(survey);
                    surveysDto.Add(surveydto.Data);
                }
                return ResponseHandler.Success(surveysDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<SurveyDTO>>($"An Error Occurred, {Ex}");
            }


        }

        #endregion

        #region GetAll
        public async Task<Response<ICollection<SurveyDTO>>> GetAllAsync()
        {
            try
            {
                var surveys = await _unitOfWork.Surveys.GetAllAsync();
                if (surveys.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<SurveyDTO>>("There are no Surveys yet");

                ICollection<SurveyDTO> surveysDto = new List<SurveyDTO>();
                foreach (var survey in surveys)
                {
                    var surveydto = _mapper.Map<SurveyDTO>(survey);
                    surveydto.Options = await GetSurveyOptions(survey.Id);
                    surveydto.groups = await GetSurveyGroups(survey.Id);
                    surveysDto.Add(surveydto);
                }
                return ResponseHandler.Success(surveysDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<SurveyDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetFromGroupId
        public async Task<Response<ICollection<SurveyDTO>>> GetFromGroups(int GroupId)
        {
            try
            {
                var surveys = await _unitOfWork.GroupSurveys.
                    GetWhereSelectAsync(v => v.GroupId == GroupId, v => v.SurveyId);
                if (surveys.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<SurveyDTO>>("There are no Surveys yet");
                ICollection<SurveyDTO> surveysDto = new List<SurveyDTO>();
                foreach (var survey in surveys)
                {
                    var surveydto = await GetByIdAsync(survey);
                    surveysDto.Add(surveydto.Data);
                }
                return ResponseHandler.Success(surveysDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<SurveyDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region Delete One
        public async Task<Response<SurveyDTO>> DeleteAsync(int Id)
        {
            var survey = await _unitOfWork.Surveys.GetByIdAsync(Id);
            if (survey is null)
                return ResponseHandler.NotFound<SurveyDTO>("There is no such Survey");
            try
            {
                var options = await _unitOfWork.Options.GetWhereAsync(opt => opt.SurveyId == Id);
                foreach (var opt in options)
                {
                    await _unitOfWork.Options.DeleteAsync(opt);
                }
                await _unitOfWork.Surveys.DeleteAsync(survey);
                return ResponseHandler.Deleted<SurveyDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<SurveyDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        private async Task SendToGroupsAsync(ICollection<int> Groups, int surveyId)
        {
            foreach (var groupId in Groups)
            {
                GroupSurvey NewGroupSurvey = new GroupSurvey()
                {
                    GroupId = groupId,
                    SurveyId = surveyId
                };
                await _unitOfWork.GroupSurveys.AddAsync(NewGroupSurvey);
            }
        }

        private async Task<ICollection<string>> GetSurveyOptions(int SurveyId)
        {
            return await _unitOfWork.Options.GetWhereSelectAsync(opt => opt.SurveyId == SurveyId, opt => opt.Text);
        }

        private async Task<ICollection<int>> GetSurveyGroups(int SurveyId)
        {
            return await _unitOfWork.GroupSurveys.GetWhereSelectAsync(g => g.SurveyId == SurveyId, g => g.GroupId);
        }*/
    }
}
