using BusinessLayer.Models;
using CoreLayer.Utilities.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract;

public interface ICurrencyService
{
    Task<IDataResult<List<CurrencysModel>>> GetCurrencyDataAsync(params string[] currencys);
}