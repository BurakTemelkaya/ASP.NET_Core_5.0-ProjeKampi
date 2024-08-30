using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using DataAccessLayer.Abstract;
using CoreLayer.CrossCuttingConcerns.Caching;

namespace BusinessLayer.Stores;

public class TicketStore : ITicketStore
{
    private readonly IUserSessionDal _userSessionDal;
    private readonly ICacheManager _cacheManager;

    public TicketStore(IUserSessionDal userSessionDal, ICacheManager cacheManager)
    {
        _userSessionDal = userSessionDal;
        _cacheManager = cacheManager;
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

        _cacheManager.Add(key, userSession, 3600);

        return key;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        UserSession session;

        session = _cacheManager.Get<UserSession>(key);

        session ??= await _userSessionDal.GetByFilterAsync(s => s.SessionKey == key);

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

            await _userSessionDal.UpdateAsync(session);

            _cacheManager.Add(key, session, 3600);
        }
    }

    public async Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        UserSession session;

        session = _cacheManager.Get<UserSession>(key);

        session ??= await _userSessionDal.GetByFilterAsync(s => s.SessionKey == key);

        if (session == null || session.ExpiresAtTime <= DateTimeOffset.UtcNow)
        {
            return null;
        }

        if (!_cacheManager.IsAdd(key))
        {
            _cacheManager.Add(key, session, 3600);
        }

        return DeserializeTicket(session.Value);
    }

    public async Task RemoveAsync(string key)
    {
        UserSession session;

        session = _cacheManager.Get<UserSession>(key);

        session ??= await _userSessionDal.GetByFilterAsync(s => s.SessionKey == key);

        if (session != null)
        {
            await _userSessionDal.DeleteAsync(session);
            _cacheManager.Remove(key);
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