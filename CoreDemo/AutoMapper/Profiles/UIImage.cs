using AutoMapper;
using BusinessLayer.Models;
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
            CreateMap<NewsLetterSendMailsModel, NewsLetterDraft>().ForMember(x => x.TimeToAdd, opt => opt.Ignore()).ForMember(x => x.NewsLetterDraftId, opt => opt.Ignore()).ReverseMap();
        }
    }
}
