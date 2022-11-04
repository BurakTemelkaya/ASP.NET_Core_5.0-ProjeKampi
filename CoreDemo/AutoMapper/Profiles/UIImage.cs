using AutoMapper;
using CoreDemo.Areas.Admin.Models;
using CoreDemo.Models;
using EntityLayer.Concrete;
using EntityLayer.DTO;

namespace CoreDemo.AutoMapper.Profiles
{
    public class UIImage : Profile
    {
        public UIImage()
        {
            CreateMap<BannedUserModel, AppUser>().ReverseMap();
            CreateMap<ResetPasswordDto, ForgotPasswordModel>().ReverseMap();
        }
    }
}
