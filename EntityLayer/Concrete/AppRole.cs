using CoreLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Concrete
{
    public class AppRole : IdentityRole<int>, IEntity
    {

    }
}
