using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete.Stores;

public class TicketStore : ITicketStore
{
    private readonly Context _context;

    public TicketStore(Context context)
    {
        _context = context;
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = Guid.NewGuid().ToString();

        var userId = ticket.Principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var userSession = new UserSession
        {
            SessionKey = key,
            UserId = userId,  // Doğru ve benzersiz User ID kullanılıyor
            Value = SerializeTicket(ticket),
            ExpiresAtTime = DateTimeOffset.UtcNow.AddDays(30),
        };

        _context.UserSessions.Add(userSession);
        await _context.SaveChangesAsync();
        return key;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var session = await _context.UserSessions.SingleOrDefaultAsync(s => s.SessionKey == key);
        if (session != null)
        {
            session.ExpiresAtTime = DateTimeOffset.UtcNow.AddDays(30);

            session.SlidingExpirationInSeconds = (long)(TimeSpan.FromDays(30).TotalSeconds);

            session.AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(60);

            session.Value = SerializeTicket(ticket);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        var session = await _context.UserSessions.SingleOrDefaultAsync(s => s.SessionKey == key);
        if (session == null || session.ExpiresAtTime <= DateTimeOffset.UtcNow)
        {
            return null;
        }

        return DeserializeTicket(session.Value);
    }

    public async Task RemoveAsync(string key)
    {
        var session = await _context.UserSessions.SingleOrDefaultAsync(s => s.SessionKey == key);
        if (session != null)
        {
            _context.UserSessions.Remove(session);
            await _context.SaveChangesAsync();
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
