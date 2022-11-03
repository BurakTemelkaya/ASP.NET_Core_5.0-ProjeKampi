using CoreLayer.Utilities.MailUtilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.MailUtilities
{
    public class MailTemplates
    {
        public static string ChangedUserInformationByAdminMailTemplate(ChangedUserInformationModel model)
        {
            return "<h2>HESABINIZIN BİLGİLERİ ADMİNLERİMİZ TARAFINDAN DEĞİŞTİRİLDİ." +
                "</h2><h3><b>ŞU ANKİ BİLGİLERİNİZ:&nbsp;</b>" +
                "</h3><h4>Kullanıcı Adı: <b>" + model.Username +
                "</b></h4></h3><h4>Ad Soyad: <b>" + model.NameSurname +
                "</b></h4><h4>E-Posta: <b>" + model.Email + "</b></h4><h3>Resim:</h3><p>" +
                "Hakkında: <b>" + model.About + "</b></h3>" +
                "<h3>Şehir: " + model.City + " <b>TEST</b></h3>";
        }
        public static string ChangedUserInformationMailTemplate(ChangedUserInformationModel model)
        {
            return "<h2>HESABINIZIN BİLGİLERİ DEĞİŞTİRİLDİ.</h2><h3><b>ŞU ANKİ BİLGİLERİNİZ:&nbsp;</b></h3><h4>Kullanıcı Adı: <b> " + model.Username + " +</b></h4></h3><h4>Ad Soyad: <b> " + model.NameSurname + "</b></h4><h4>E-Posta: <b>" + model.Email + "</b></h4><h3>Resim:</h3><p>Hakkında: <b>" + model.About + "</b></h3><h3>Şehir: " + model.City + " <b>TEST</b></h3>";
        }
        public static string ChangedUserInformationMailSubject()
        {
            return "Core Blog hesabınızın bilgileri değiştirildi.";
        }
        public static string BanOpenUserContentTemplate()
        {
            return "Core Blog hesabınızın yasağı adminlerimiz tarafından kaldırılmıştır.";
        }
        public static string BanOpenUserSubjectTemplate()
        {
            return "Core Blog hesabınızın yasağı kaldırıldı";
        }
        public static string BanMessageSubject()
        {
            return "Core Blog hesabınız yasaklandı";
        }
        public static string BanMessageContent(DateTime banExpiration)
        {
            return "Sitemizin kurallarını ihlal ettiğiniz için hesabınız " + banExpiration.ToString() + " tarihine kadar yasaklanmıştır";
        }
    }
}
