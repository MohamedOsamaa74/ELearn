using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Helpers.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Announcement Mapper
            CreateMap<Announcement, AnnouncementDTO>()
                .ForMember(dest => dest.Groups, opt => opt.Ignore());

            CreateMap<AnnouncementDTO, Announcement>();
            #endregion

            #region User Mapper
            CreateMap<ApplicationUser, UserDTO>();

            CreateMap<UserDTO, ApplicationUser>();

            CreateMap<EditUserDTO, ApplicationUser>();
            #endregion

            #region Group Mapper
            CreateMap<Group, GroupDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GroupName));

            CreateMap<GroupDTO, Group>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Name));
            #endregion

            #region Material Mapper
            CreateMap<UpdateMaterialDTO, Material>()
                .ForMember(dest => dest.GroupId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());


            CreateMap<Material, UpdateMaterialDTO>()
            .ForMember(dest => dest.File, opt => opt.Ignore())
            .ForMember(dest => dest.Link, opt => opt.Ignore());
            #endregion
        }
    }
}