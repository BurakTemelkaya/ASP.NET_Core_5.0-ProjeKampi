namespace CoreLayer.Utilities.MailUtilities
{
    public interface IMailService
    {
        bool SendMails(string[] mail, string subject, string message);
        bool SendMail(string mail, string subject, string message);
    }
}
