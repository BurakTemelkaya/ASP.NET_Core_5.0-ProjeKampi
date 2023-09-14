using AutoMapper;
using EntityLayer.DTO;
using EntityLayer.Concrete;
using BusinessLayer.Models;

namespace BusinessLayer.AutoMapper.Profiles
{
    public class BusinessImages : Profile
    {
        public BusinessImages()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<ChangedUserInformationModel, AppUser>().ReverseMap();
            CreateMap<UserSignUpDto, AppUser>().ReverseMap();
            CreateMap<NewsLetterSendMailsModel, NewsLetterDraft>().ReverseMap();
        }
    }
}
