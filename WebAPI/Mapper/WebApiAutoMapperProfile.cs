using AutoMapper;
using Azure;
using DAL.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAPI.Dtos;

namespace WebAPI.Mapper
{
    public class WebApiAutoMapperProfile : Profile
    {
        public WebApiAutoMapperProfile()
        {
            CreateMap<Poll, PollDto>();
            CreateMap<PollDto, Poll>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserAnswer, UserAnswerDto>();
            CreateMap<UserAnswerDto, UserAnswer>();
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionDto, Question>();
        }
    }
}
