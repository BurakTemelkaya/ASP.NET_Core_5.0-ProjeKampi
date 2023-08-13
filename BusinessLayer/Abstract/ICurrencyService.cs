using BusinessLayer.Constants;
using BusinessLayer.Models;
using CoreLayer.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICurrencyService
    {
        public IDataResult<List<CurrencysModel>> GetCurrencys(params string[] currencys);
    }
}
