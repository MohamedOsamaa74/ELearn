using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.DTOs.QuizDTOs;
using ELearn.Application.DTOs.SurveyDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.InfraStructure.Validations;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;

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
        public async Task<Response<ViewSurveyDTO>> CreateNewAsync(CreateSurveyDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var survey = _mapper.Map<Survey>(Model);
                survey.CreatorId = user.Id;
                var validation = new SurveyValidation().Validate(survey);
                if (!validation.IsValid)
                    return ResponseHandler.BadRequest<CreateSurveyDTO>(null,validation.Errors.Select(x => x.ErrorMessage).ToList());
                await _unitOfWork.Surveys.AddAsync(survey);
                
                var result = await SendToGroups(survey.Id, Model.GroupIds);
                foreach (var question in Model.Questions)
                {
                    await _questionService.CreateNewAsync(question, "Survey", survey.Id);
                }
                var surveyDto = _mapper.Map<ViewSurveyDTO>(survey);
                surveyDto.Id = survey.Id;
                surveyDto.CreatorName = user.FirstName + ' ' + user.LastName;
                if (result != "Success")
                    return ResponseHandler.BadRequest<ViewSurveyDTO>(result);
                return ResponseHandler.Success(surveyDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ViewSurveyDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetById
        public async Task<Response<ViewSurveyDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var survey = await _unitOfWork.Surveys.GetByIdAsync(Id);
                if (survey is null)
                    return ResponseHandler.NotFound<ViewSurveyDTO>("There is no such Survey");
                var creator = await _userService.GetByIdAsync(survey.CreatorId);
                var surveyDto = _mapper.Map<ViewSurveyDTO>(survey);
                var Questions = await _unitOfWork.Questions.GetWhereAsync(q => q.SurveyId == Id);
                surveyDto.Questions = _mapper.Map<ICollection<QuestionDTO>>(Questions);
                surveyDto.CreatorName = creator.FirstName + ' ' + creator.LastName;
                return ResponseHandler.Success(surveyDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ViewSurveyDTO>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        #region Delete
        public async Task<Response<CreateSurveyDTO>> DeleteAsync(int Id)
        {
            try
            {
                var survey = await _unitOfWork.Surveys.GetByIdAsync(Id);
                if (survey is null)
                    return ResponseHandler.NotFound<CreateSurveyDTO>("There is no such Survey");
                await _unitOfWork.Surveys.DeleteAsync(survey);
                return ResponseHandler.Deleted<CreateSurveyDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<CreateSurveyDTO>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        #region DeleteMany
        public async Task<Response<CreateSurveyDTO>> DeleteManyAsync(int[] Ids)
        {
            try
            {
                foreach (var Id in Ids)
                {
                    var survey = await _unitOfWork.Surveys.GetByIdAsync(Id);
                    if (survey is null)
                        return ResponseHandler.NotFound<CreateSurveyDTO>("There is no such Survey");
                    await _unitOfWork.Surveys.DeleteAsync(survey);
                }
                return ResponseHandler.Deleted<CreateSurveyDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<CreateSurveyDTO>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        #region GetAll
        public async Task<Response<ICollection<ViewSurveyDTO>>> GetAllAsync()
        {
            try
            {
                var surveys = await _unitOfWork.Surveys.GetAllAsync();
                if (surveys.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewSurveyDTO>>("There are no Surveys yet");

                ICollection<ViewSurveyDTO> surveysDto = [];
                foreach (var survey in surveys)
                {
                    var surveydto = _mapper.Map<ViewSurveyDTO>(survey);
                    var creator = await _userService.GetByIdAsync(survey.CreatorId);
                    surveydto.CreatorName = creator.FirstName + ' ' + creator.LastName;
                    surveydto.Questions = _mapper.Map<ICollection<QuestionDTO>>(await _unitOfWork.Questions.GetWhereAsync(q => q.SurveyId == survey.Id));
                    surveysDto.Add(surveydto);
                }
                return ResponseHandler.Success(surveysDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewSurveyDTO>>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        #region GetFromGroup
        public async Task<Response<ICollection<ViewSurveyDTO>>> GetFromGroup(int GroupId)
        {
            try
            {
                var surveys = await _unitOfWork.GroupSurveys.GetWhereSelectAsync(g => g.GroupId == GroupId, g => g.SurveyId);
                if (surveys.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewSurveyDTO>>("There are no Surveys yet");
                ICollection<ViewSurveyDTO> surveysDto = [];
                foreach (var survey in surveys)
                {
                    var surveydto = GetByIdAsync(survey);
                    surveysDto.Add(surveydto.Result.Data);
                }
                return ResponseHandler.Success(surveysDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewSurveyDTO>>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        #region GetFromUserGroups
        public async Task<Response<ICollection<ViewSurveyDTO>>> GetFromUserGroups()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var groups = await _unitOfWork.UserGroups.GetWhereSelectAsync(u => u.UserId == user.Id, g => g.GroupId);
                if (groups.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewSurveyDTO>>("You Are Not in Any Groups");

                var surveys = new HashSet<int>(); // Use HashSet to ensure uniqueness
                foreach (var group in groups)
                {
                    var groupSurveys = await _unitOfWork.GroupSurveys.GetWhereSelectAsync(v => v.GroupId == group, v => v.SurveyId);
                    foreach (var surveyId in groupSurveys)
                    {
                        surveys.Add(surveyId); // HashSet.Add ignores duplicates
                    }
                }

                if (surveys.Count == 0) // Check if HashSet is empty
                    return ResponseHandler.NotFound<ICollection<ViewSurveyDTO>>("There are no Surveys yet");

                ICollection<ViewSurveyDTO> surveysDto = []; // Initialize the list
                foreach (var survey in surveys)
                {
                    var surveydto = await GetByIdAsync(survey); // Assuming GetByIdAsync is an async method
                    surveysDto.Add(surveydto.Data);
                }

                return ResponseHandler.Success(surveysDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewSurveyDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetByCreator
        public async Task<Response<ICollection<ViewSurveyDTO>>> GetSurveysByCreator()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var surveys = await _unitOfWork.Surveys.GetWhereSelectAsync(v => v.CreatorId == user.Id, v => v.Id);
                if (surveys.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewSurveyDTO>>("There are No Surveys yet");
                ICollection<ViewSurveyDTO> surveysDto = [];
                foreach (var survey in surveys)
                {
                    var surveydto = await GetByIdAsync(survey);
                    surveysDto.Add(surveydto.Data);
                }
                return ResponseHandler.Success(surveysDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewSurveyDTO>>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        #region RecieveStudentResponse
        public async Task<Response<UserAnswerSurveyDTO>> SubmitResponseAsync(UserAnswerSurveyDTO userAnswerDTO) 
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var survey = await _unitOfWork.Surveys.GetByIdAsync(userAnswerDTO.SurveyId);
                if (survey is null)
                    return ResponseHandler.NotFound<UserAnswerSurveyDTO> ("There is no such Survey");

                foreach (var answer in userAnswerDTO.Answers)
                {

                    var recieveAnswer = await _questionService.RecieveStudentAnswerAsync(_mapper.Map<QuestionQuizDTO>(answer));
                    if(!recieveAnswer.Succeeded)
                        return ResponseHandler.BadRequest<UserAnswerSurveyDTO>($"An Error Occurred, {recieveAnswer.Message}");
                    
                    
                }
                UserAnswerSurvey userAnswerSurvey= new() { SurveyId = userAnswerDTO.SurveyId, UserId = user.Id };
                await _unitOfWork.UserAnswerSurveys.AddAsync(userAnswerSurvey);
                return ResponseHandler.Success(userAnswerDTO);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<UserAnswerSurveyDTO>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

        #region GetUserAnswer
        public async Task<Response<UserAnswerSurveyDTO>> GetUserAnswerAsync(int surveyId, string UserId)
        {
            try
            {
                var user = await _userService.GetByIdAsync(UserId);
                if (user is null)
                    return ResponseHandler.NotFound<UserAnswerSurveyDTO>("There is no such User");
                var survey = await _unitOfWork.Surveys.GetByIdAsync(surveyId);
                if (survey is null)
                    return ResponseHandler.NotFound<UserAnswerSurveyDTO>("There is no such Survey");
                var studentAnswers = await _questionService.GetStudentAnswersAsync("Survey", surveyId, UserId);
                var userAnswers = new UserAnswerSurveyDTO()
                {
                    SurveyId = surveyId,
                    Answers = studentAnswers.Data
                };
                return ResponseHandler.Success(userAnswers);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<UserAnswerSurveyDTO>($"An Error Occorred, {Ex}");
            }
        }
        #endregion

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

    }
}
