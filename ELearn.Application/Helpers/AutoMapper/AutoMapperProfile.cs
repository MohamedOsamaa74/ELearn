using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.DTOs.AnnouncementDTOs;
using ELearn.Application.DTOs.AssignmentDTOs;
using ELearn.Application.DTOs.CommentDTOs;
using ELearn.Application.DTOs.DepartementDTOs;
using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.DTOs.GroupDTOs;
using ELearn.Application.DTOs.MaterialDTOs;
using ELearn.Application.DTOs.MessageDTOs;
using ELearn.Application.DTOs.OptionDTOs;
using ELearn.Application.DTOs.PostDTOs;
using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.DTOs.QuizDTOs;
using ELearn.Application.DTOs.ReactDTOs;
using ELearn.Application.DTOs.SurveyDTOs;
using ELearn.Application.DTOs.UserDTOs;
using ELearn.Application.DTOs.VotingDTOs;
using ELearn.Domain.Entities;

namespace ELearn.Application.Helpers.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Announcement Mapper
            CreateMap<Announcement, UploadAnnouncementDTO>()
                .ForMember(dest => dest.Groups, opt => opt.Ignore());

            CreateMap<UploadAnnouncementDTO, Announcement>()
                .ForMember(dest => dest.Files, opt => opt.Ignore());

            CreateMap<UploadAnnouncementDTO, ViewAnnouncementDTO>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.FilesUrls, opt => opt.Ignore());

            CreateMap<Announcement, ViewAnnouncementDTO>()
                .ForMember(dest => dest.FilesUrls, opt => opt.Ignore())
                .ForMember(dest => dest.Groups, opt => opt.Ignore());
            #endregion

            #region User Mapper
            CreateMap<ApplicationUser, AddUserDTO>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<AddUserDTO, ApplicationUser>();

            CreateMap<EditUserDTO, ApplicationUser>();

            CreateMap<ApplicationUser, UserProfileDTO>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePictureName, opt => opt.Ignore());

            CreateMap<ApplicationUser, ParticipantDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName));
            #endregion

            #region Departement Mapper

            CreateMap<Department, ViewDepartementDTO>();

            #endregion

            #region Group Mapper
            CreateMap<Group, GroupDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<GroupDTO, Group>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            #endregion

            #region Voting Mapper
            CreateMap<Voting, AddVotingDTO>()
                .ForMember(dest => dest.groups, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.Ignore());

            CreateMap<AddVotingDTO, Voting>()
                .ForMember(dest => dest.CreatorId, opt => opt.Ignore());
            //.ForMember(dest => dest.Options, opt => opt.Ignore())

            CreateMap<Voting, ViewVotingDTO>()
                .ForMember(dest => dest.Groups, opt => opt.Ignore())
                .ForMember(dest => dest.CreatorName, opt => opt.Ignore());

            CreateMap<ViewVotingDTO, Voting>();

            CreateMap<AddVotingDTO, ViewVotingDTO>();

            #endregion

            #region Material Mapper
            CreateMap<Material, AddMaterialDTO>()
                .ForMember(dest => dest.File, opt => opt.Ignore());

            CreateMap<AddMaterialDTO, Material>()
                .ForMember(dest => dest.GroupId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<UpdateMaterialDTO, Material>()
                .ForMember(dest => dest.GroupId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Material, UpdateMaterialDTO>()
                .ForMember(dest => dest.File, opt => opt.Ignore())
                .ForMember(dest => dest.Link, opt => opt.Ignore());

            CreateMap<Material, MaterialDTO>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.DownloadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ViewUrl, opt => opt.Ignore());
            #endregion

            #region Assignment Mapper 
            CreateMap<UploadAssignmentDTO, Assignment>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore());

            CreateMap<Assignment, UploadAssignmentDTO>()
                .ForMember(dest => dest.Attachements, opt => opt.Ignore());

            CreateMap<Assignment, ViewAssignmentDTO>()
                .ForMember(dest => dest.CreatorName, opt => opt.Ignore())
                .ForMember(dest => dest.FilesURLs,  opt => opt.Ignore());

            CreateMap<UserAnswerAssignment, ViewAssignmentResponseDTO>()
                .ForMember(dest => dest.Mark, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.UploadDate, opt => opt.Ignore())
                .ForMember(dest => dest.UploadTime, opt => opt.Ignore())
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.FileURL, opt => opt.Ignore());
            #endregion

            #region Quiz Mapper
            CreateMap<CreateQuizDTO, Quiz>()
                .ForMember(dest => dest.GroupId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<Quiz, CreateQuizDTO>();

            CreateMap<Quiz, EditQuizDTO>();

            CreateMap<EditQuizDTO, Quiz>()
                .ForMember(dest => dest.GroupId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore());


            CreateMap<ViewQuizDTO, Quiz>()
              .ForMember(dest => dest.GroupId, opt => opt.Ignore())
              .ForMember(dest => dest.UserId, opt => opt.Ignore())
              .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<Quiz, ViewQuizDTO>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            

            CreateMap<QuizResultDTO, Quiz>()
            .ForMember(dest => dest.GroupId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<Quiz, QuizResultDTO>()
                .ForMember(dest => dest.QuestionAnswers, opt => opt.Ignore());




            #endregion

            #region Question Mapper
            CreateMap<QuestionDTO, Question>()
                .ForMember(dest => dest.QuizId, opt => opt.Ignore())
                .ForMember(dest => dest.SurveyId, opt => opt.Ignore());

            CreateMap<Question, QuestionDTO>();

            CreateMap<QuestionAnswerDTO, UserAnswerQuestion>();

            CreateMap<UserAnswerQuestion, QuestionAnswerDTO>()
                .ForMember(dest => dest.Score, opt => opt.Ignore());

            CreateMap<QuestionQuizDTO, QuestionAnswerDTO>()
                .ForMember(dest => dest.Score, opt => opt.Ignore());

            CreateMap<QuestionAnswerDTO, QuestionQuizDTO>();


            CreateMap<UserAnswerQuestion, QuestionQuizDTO>();

            CreateMap<QuestionQuizDTO, UserAnswerQuestion>();

            CreateMap<UserAnswerQuestion, QuestionAnswerDTO>()
                .ForMember(dest => dest.Score, opt => opt.Ignore());

            CreateMap<QuestionAnswerDTO, UserAnswerQuestion>();
            #endregion

            #region File Mapper
            CreateMap<FileEntity, FileDTO>();
            #endregion

            #region Survey Mapper
            CreateMap<Survey, CreateSurveyDTO>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<CreateSurveyDTO, Survey>();

            CreateMap<Survey, ViewSurveyDTO>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.CreatorName, opt => opt.Ignore());

            CreateMap<ViewSurveyDTO, Survey>();

            CreateMap<CreateSurveyDTO, ViewSurveyDTO>()
                .ForMember(dest => dest.Id, src => src.Ignore())
                .ForMember(dest => dest.CreatorName, src => src.Ignore());
            #endregion

            #region PostMapper
            CreateMap<CreatePostDTO, Post>()
                .ForMember(dest => dest.Files, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<Post, CreatePostDTO>();

            CreateMap<Post, ViewPostDTO>()
                .ForMember(dest => dest.CreatorName, opt => opt.Ignore())
                .ForMember(dest => dest.urls, opt => opt.Ignore());
            CreateMap<ViewPostDTO, Post>()
                .ForMember(dest => dest.Files, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());


            #endregion

            #region CommentMapper

            CreateMap<CreateCommentDTO,Comment>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<Comment, CreateCommentDTO>();

            CreateMap<Comment, ViewCommentDTO>()
                .ForMember(dest => dest.CreatorName, opt => opt.Ignore());
            CreateMap<Comment, ViewCommentDTO>();


            

            CreateMap<UpdateCommentDTO, Comment>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<Comment, UpdateCommentDTO>();

            CreateMap<Comment, ViewCommentDTO>()
                .ForMember(dest => dest.CreatorName, opt => opt.Ignore());
            CreateMap<Comment, ViewCommentDTO>();








            #endregion

            #region React Mapper
            CreateMap<ReactDTO, React>()
                .ForMember(c => c.CreationDate, opt => opt.Ignore())
                .ForMember(p => p.PostID, opt => opt.Ignore())
                .ForMember(cm => cm.CommentId, opt => opt.Ignore())
                .ForMember(u => u.UserID, opt => opt.Ignore());

            CreateMap<React, ReactDTO>()
                .ForMember(p => p.ParentId, opt => opt.Ignore())
                .ForMember(pn => pn.Parent, opt => opt.Ignore());
            #endregion

            #region MessageMapper

            CreateMap<SendMessageDTO, Message>()
                .ForMember(dest => dest.File, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverId, opt => opt.Ignore())
                .ForMember(dest => dest.SenderId, opt => opt.Ignore());

            CreateMap<Message, SendMessageDTO>();

            CreateMap<Message, ViewMessageDTO>()
                .ForMember(dest => dest.url, opt => opt.Ignore());
            CreateMap<ViewMessageDTO, Message>();
            #endregion 
        }
    }
}