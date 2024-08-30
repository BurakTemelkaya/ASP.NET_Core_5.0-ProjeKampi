using CoreLayer.Entities;
using System;

namespace EntityLayer.Concrete;

public class UserSession : IEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string SessionKey { get; set; }
    public byte[] Value { get; set; }
    public DateTimeOffset ExpiresAtTime { get; set; }
    public long? SlidingExpirationInSeconds { get; set; }
    public DateTimeOffset? AbsoluteExpiration { get; set; }
}
