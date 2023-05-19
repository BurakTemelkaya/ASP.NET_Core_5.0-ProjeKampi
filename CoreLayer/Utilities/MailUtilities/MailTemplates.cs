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
                "<h3>Şehir: <b>" + model.City + "</b></h3>";
        }
        public static string ChangedUserInformationMailTemplate(ChangedUserInformationModel model)
        {
            return "<h2>HESABINIZIN BİLGİLERİ DEĞİŞTİRİLDİ.</h2><h3><b>ŞU ANKİ BİLGİLERİNİZ:&nbsp;</b></h3><h4>Kullanıcı Adı: <b> " + model.Username + " </b></h4></h3><h4>Ad Soyad: <b> " + model.NameSurname + "</b></h4><h4>E-Posta: <b>" + model.Email + "</b></h4><p>Hakkında: <b>" + model.About + "</b></h3><h3>Şehir:<b> " + model.City + " </b></h3>";
        }
        public static string ChangedUserInformationMailSubject()
        {
            return "Core Blog hesabınızın bilgileri değiştirildi.";
        }
        public static string BanOpenUserContentTemplate()
        {
            return "<h2>Core Blog hesabınızın yasağı adminlerimiz tarafından kaldırılmıştır.</h2>";
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
            return "<h2>Sitemizin kurallarını ihlal ettiğiniz için hesabınız " + banExpiration.ToString() + " tarihine kadar yasaklanmıştır</h2>";
        }
        public static string ResetPasswordSubject()
        {
            return "Core Blog Hesabınızın Parolasını Sıfırlayın";
        }
        public static string ResetPasswordContent(string link)
        {
            return "<h2>Hesabının parolasını bu bağlantıya tıklayarak sıfırlayabilirsin. Link:&nbsp;<a href=\"" + link + "\">Bağlantı</a></h2>";
        }
        public static string ResetPasswordInformationSubject()
        {
            return "Hesabınızın Parolası Değiştirildi.";
        }
        public static string ResetPasswordInformationMessage()
        {
            return "<h2><b>Hesabınızın parolası değiştirildi.</b>Eğer bu işlemi siz yaptıysanız maili görmezden gelebilirsiniz Eğer siz yapmadıysanız hemen bizimle iletişime geçin.</h2>";
        }

        public static string ConfirmEmailSubject()
        {
            return "Core Blog Mail Doğrulaması";
        }

        public static string ConfirmEmailMessage(string link)
        {
            return "<a href=" + link + "> <h2><b>E-Posta doğrulaması yapmak için tıklayın.</h2></b> <a/> Eğer link çalışmıyorsa buradan linke ulaşabilirsin: " + link;
        }
    }
}
