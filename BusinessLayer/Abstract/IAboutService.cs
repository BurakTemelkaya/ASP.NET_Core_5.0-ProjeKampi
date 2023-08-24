using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAboutService
    {

        Task<IResultObject> UpdateAsync(About t, IFormFile aboutImage1, IFormFile aboutImage2);

        Task<IDataResult<About>> GetAboutAsync();

        Task<IDataResult<About>> GetAboutByFooterAsync();

        Task<IDataResult<About>> GetAboutByUpdateAsync();
    }
}
