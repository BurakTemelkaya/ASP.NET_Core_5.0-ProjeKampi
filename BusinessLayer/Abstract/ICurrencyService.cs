using BusinessLayer.Constants;
using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICurrencyService
    {
        public List<CurrencysModel> GetCurrencys(params string[] currencys);
    }
}
