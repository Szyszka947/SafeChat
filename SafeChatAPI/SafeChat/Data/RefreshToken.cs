using System;

namespace SafeChatAPI.Data
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsExpired;
        public UserEntity User { get; set; }
    }
}
