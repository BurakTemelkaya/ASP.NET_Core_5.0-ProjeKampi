using AutoMapper;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Results;
using EntityLayer.DTO;

namespace BusinessLayer.Concrete
{
    public class ManagerBase
    {
        public ManagerBase(IMapper mapper)
        {
            Mapper = mapper;
        }
        protected IMapper Mapper { get; }

        public IResultObject UserNotEmpty(IDataResult<UserDto> user)
        {
            if (!user.Success)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            return new SuccessResult();
        }
    }
}
