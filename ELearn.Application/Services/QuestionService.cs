﻿using AutoMapper;
using ELearn.Application.DTOs.QuestionDTOs;
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
    public class QuestionService : IQuestionService
    {
        #region Fields and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region CreateNewAsync
        public async Task<Response<QuestionDTO>> CreateNewAsync(QuestionDTO Model, string Parent, int ParentId)
        {
            try
            {
                var question = _mapper.Map<Question>(Model);
                if (Parent == "Quiz")
                {
                    var quiz = await _unitOfWork.Quizziz.GetByIdAsync(ParentId);
                    if(quiz == null)
                        return ResponseHandler.BadRequest<QuestionDTO>("Quiz is not valid");
                    question.QuizId = ParentId;
                }
                else if (Parent == "Survey")
                {
                    var survey = await _unitOfWork.Surveys.GetByIdAsync(ParentId);
                    if (survey == null)
                        return ResponseHandler.BadRequest<QuestionDTO>("Survey is not valid");
                    question.SurveyId = ParentId;
                }
                else
                    return ResponseHandler.BadRequest<QuestionDTO>("Parent is not valid");

                await _unitOfWork.Questions.AddAsync(question);
                return ResponseHandler.Success(Model);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<QuestionDTO>(ex.Message);
            }
        }
        #endregion

        #region UpdateAsync
        public async Task<Response<QuestionDTO>> UpdateAsync(int Id, QuestionDTO Model)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(Id);
                if (question == null)
                    return ResponseHandler.NotFound<QuestionDTO>("Question not found");

                question = _mapper.Map(Model, question);
                await _unitOfWork.Questions.UpdateAsync(question);
                return ResponseHandler.Success(Model);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<QuestionDTO>(ex.Message);
            }
        }
        #endregion

        #region DeleteAsync
        public async Task<Response<QuestionDTO>> DeleteAsync(int Id)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(Id);
                if (question == null)
                    return ResponseHandler.NotFound<QuestionDTO>("Question not found");

                await _unitOfWork.Questions.DeleteAsync(question);
                return ResponseHandler.Deleted<QuestionDTO>();
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<QuestionDTO>(ex.Message);
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<Response<QuestionDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(Id);
                if (question == null)
                    return ResponseHandler.NotFound<QuestionDTO>("Question not found");

                var questionDTO = _mapper.Map<QuestionDTO>(question);
                return ResponseHandler.Success(questionDTO);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<QuestionDTO>(ex.Message);
            }
        }
        #endregion
    }
}
