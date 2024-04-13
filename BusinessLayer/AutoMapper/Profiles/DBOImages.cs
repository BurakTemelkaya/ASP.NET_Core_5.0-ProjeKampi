using AutoMapper;
using EntityLayer.Concrete;

namespace BusinessLayer.AutoMapper.Profiles
{
    public class DBOImages : Profile
    {
        public DBOImages()
        {
            CreateMap<MessageDraft, Message>()
                .ForPath(x => x.ReceiverUser.UserName, y => y.MapFrom(z => z.ReceiverUser)).ReverseMap();
        }
    }
}
