using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface ISurveyService
    {
        public Task<Response<SurveyDTO>> CreateNewAsync(SurveyDTO Model);
        public Task<Response<SurveyDTO>> GetByIdAsync(int Id);
        public Task<Response<ICollection<SurveyDTO>>> GetSurveysByCreator();
        public Task<Response<ICollection<SurveyDTO>>> GetFromUserGroups();
        public Task<Response<ICollection<SurveyDTO>>> GetAllAsync();
        public Task<Response<ICollection<SurveyDTO>>> GetFromGroups(int GroupId);
        public Task<Response<SurveyDTO>> DeleteAsync(int Id);
    }
}
