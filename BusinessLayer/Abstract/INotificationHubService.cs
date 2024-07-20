using System.Threading.Tasks;

namespace BusinessLayer.Abstract;

public interface INotificationHubService
{
    Task SendNotification(int userId, string message);
}
