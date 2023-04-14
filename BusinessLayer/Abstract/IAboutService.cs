using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAboutService
    {

        Task<IResult> TUpdateAsync(About t);

        Task<IDataResult<About>> GetFirst();
    }
}
