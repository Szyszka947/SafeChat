using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace SafeChatAPI.Data.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<GroupInvitationEntity> GroupInvitations { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<GroupAdminEntity> GroupsAdmins { get; set; }
        public DbSet<GroupBanEntity> GroupBans { get; set; }
    }
}
