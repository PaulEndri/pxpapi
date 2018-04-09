#region

using PixelPubApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

#endregion

namespace PixelPubApi.MySQL
{
    public class WrathIncarnateContext : DbContext
    {

        public WrathIncarnateContext(DbContextOptions<WrathIncarnateContext> options)
            : base(options)
        { }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<ApiAccess> ApiAccess { get; set; }
    }
}