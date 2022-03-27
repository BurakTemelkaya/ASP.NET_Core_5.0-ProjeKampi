using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ManagerBase
    {
        public ManagerBase(IMapper mapper)
        {
            Mapper = mapper;
        }
        protected IMapper Mapper { get; }
    }
}
