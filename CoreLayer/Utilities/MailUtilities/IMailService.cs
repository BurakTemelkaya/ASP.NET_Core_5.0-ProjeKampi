using System.Threading;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.MailUtilities;

public interface IMailService
{
    Task SendEmailAsync(Mail mail, CancellationToken cancellationToken = default);
}
