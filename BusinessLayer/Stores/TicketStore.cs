using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using DataAccessLayer.Abstract;

namespace BusinessLayer.Stores;

public class TicketStore : ITicketStore
{
    private readonly IUserSessionDal _userSessionDal;

    public TicketStore(IUserSessionDal userSessionDal)
    {
        _userSessionDal = userSessionDal;
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = Guid.NewGuid().ToString();

        var userId = ticket.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userSession = new UserSession
        {
            SessionKey = key,
            UserId = userId,
            Value = SerializeTicket(ticket),
            ExpiresAtTime = DateTimeOffset.UtcNow.AddDays(30),
            SlidingExpirationInSeconds = (long)(TimeSpan.FromDays(30).TotalSeconds),
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(60)
        };

        await _userSessionDal.InsertAsync(userSession);

        return key;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        UserSession session = await _userSessionDal.GetByFilterAsync(s => s.SessionKey == key);

        if (session != null)
        {
            session.ExpiresAtTime = DateTimeOffset.UtcNow.AddDays(30);

            if (session.SlidingExpirationInSeconds.HasValue)
            {
                session.SlidingExpirationInSeconds = (long)(TimeSpan.FromDays(30).TotalSeconds);
            }

            if (session.AbsoluteExpiration.HasValue)
            {
                session.AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(60);
            }

            session.Value = SerializeTicket(ticket);

            await _userSessionDal.SaveChangesAsync();
        }
    }

    public async Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        UserSession session = await _userSessionDal.GetByFilterAsync(s => s.SessionKey == key);

        if (session == null || session.ExpiresAtTime <= DateTimeOffset.UtcNow)
        {
            return null;
        }

        return DeserializeTicket(session.Value);
    }

    public async Task RemoveAsync(string key)
    {
        UserSession session = await _userSessionDal.GetByFilterAsync(s => s.SessionKey == key);

        if (session != null)
        {
            await _userSessionDal.DeleteAsync(session);
        }
    }

    private byte[] SerializeTicket(AuthenticationTicket ticket)
    {
        return TicketSerializer.Default.Serialize(ticket);
    }

    private AuthenticationTicket DeserializeTicket(byte[] bytes)
    {
        return TicketSerializer.Default.Deserialize(bytes);
    }
}