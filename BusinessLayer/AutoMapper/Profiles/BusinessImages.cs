using AutoMapper;
using BusinessLayer.Models;
using EntityLayer.Concrete;
using EntityLayer.DTO;

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
