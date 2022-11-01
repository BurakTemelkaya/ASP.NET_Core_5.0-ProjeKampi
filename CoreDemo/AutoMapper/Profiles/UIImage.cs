using AutoMapper;
using CoreDemo.Areas.Admin.Models;
using EntityLayer.Concrete;

namespace CoreDemo.AutoMapper.Profiles
{
    public class UIImage : Profile
    {
        public UIImage()
        {
            CreateMap<BannedUserModel, AppUser>().ReverseMap();
        }
    }
}
