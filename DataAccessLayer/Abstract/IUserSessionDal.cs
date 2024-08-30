using CoreLayer.DataAccess;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract;

public interface IUserSessionDal : IEntityRepository<UserSession>
{
}
