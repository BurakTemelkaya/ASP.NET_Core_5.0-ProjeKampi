using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.Models;
using CoreLayer.Aspects.AutoFac.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BusinessLayer.Concrete
{
    internal class CurrencyManager : ICurrencyService
    {
        [CacheAspect(10)]
        public List<CurrencysModel> GetCurrencys(params string[] currencys)
        {
            try
            {
                string exchangeRate = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(exchangeRate);
                var model = new List<CurrencysModel>();
                foreach (var currency in currencys)
                {
                    model.Add(new CurrencysModel
                    {
                        Code = currency,
                        Value = xmlDoc.SelectSingleNode($"Tarih_Date/Currency [@Kod='{currency}']/BanknoteSelling").InnerXml[..5].ToString()
                    });
                }
                return model;
            }
            catch
            {
                return null;
            }
        }
    }
}
