using AutoMapper;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
