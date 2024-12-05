using AutoMapper;
using DAL.Models;
using Humanizer.Localisation;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class WebAppAutoMapperProfile : Profile
    {
        public WebAppAutoMapperProfile()
        {
            CreateMap<Poll, VMPoll>();
            CreateMap<VMPoll, Poll>();
            CreateMap<Question, VMQuestion>();
            CreateMap<VMQuestion, Question>();
            CreateMap<User, VMUser>();
            CreateMap<VMUser, User>();
        }
    }
}
