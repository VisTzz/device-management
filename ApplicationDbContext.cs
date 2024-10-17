
using DevicesManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevicesManagement
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<DeviceDTO> Devices { get; set; }
    }
}
