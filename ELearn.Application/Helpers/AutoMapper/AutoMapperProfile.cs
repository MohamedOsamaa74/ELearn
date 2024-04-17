using AutoMapper;
using ELearn.Application.DTOs.AnnouncementDTOs;
using ELearn.Application.DTOs.AssignmentDTOs;
using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.DTOs.GroupDTOs;
using ELearn.Application.DTOs.MaterialDTOs;
using ELearn.Application.DTOs.QuizDTOs;
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
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.FilesUrls, opt => opt.Ignore())
                .ForMember(dest => dest.Groups, opt => opt.Ignore());
            #endregion

            #region User Mapper
            CreateMap<ApplicationUser, AddUserDTO>();

            CreateMap<AddUserDTO, ApplicationUser>();

            CreateMap<EditUserDTO, ApplicationUser>();
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
                .ForMember(dest => dest.CreatorId, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.Ignore())
                .ForMember(dest => dest.Group, opt => opt.Ignore());
            #endregion

            #region Material Mapper
            CreateMap<UpdateMaterialDTO, Material>()
                .ForMember(dest => dest.GroupId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());



            CreateMap<Material, UpdateMaterialDTO>()
            .ForMember(dest => dest.File, opt => opt.Ignore())
            .ForMember(dest => dest.Link, opt => opt.Ignore());
            #endregion

            #region Assignment Mapper 
            CreateMap<AssignmentDTO, Assignment>()
                .ForMember(dest => dest.GroupId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());



            CreateMap<Assignment, AssignmentDTO>()
            .ForMember(dest => dest.File, opt => opt.Ignore());

            #endregion

            #region Quiz Mapper
            CreateMap<CreateQuizDTO, Quiz>()
                .ForMember(dest => dest.GroupId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<Quiz, CreateQuizDTO>();
            #endregion

            #region File Mapper
            CreateMap<FileEntity, FileDTO>();
            #endregion
        }
    }
}