using BusinessLayer.Models;
using CoreLayer.Utilities.Results;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface ICurrencyService
    {
        public IDataResult<List<CurrencysModel>> GetCurrencys(params string[] currencys);
    }
}
