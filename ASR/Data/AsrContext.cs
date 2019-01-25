using ASR.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASR.Data
{
    public class AsrContext : IdentityDbContext<AppUser>
    {
        public AsrContext(DbContextOptions<AsrContext> options) : base(options)
        { }

        public DbSet<Room> Room { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Slot> Slot { get; set; }
    }
}
