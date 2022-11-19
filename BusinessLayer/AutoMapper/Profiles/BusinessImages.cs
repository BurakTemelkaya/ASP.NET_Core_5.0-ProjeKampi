using AutoMapper;
using EntityLayer.DTO;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Utilities.MailUtilities.Models;
using BusinessLayer.Models;
using BusinessLayer.Concrete;

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
