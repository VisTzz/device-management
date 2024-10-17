using DevicesManagement.Domain.Entities;
using DevicesManagement.Domain.Interfaces;

namespace DevicesManagement.Infrastructure
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeviceDTO> GetByIdAsync(int id)
        {
            return await _context.Devices.FindAsync(id);
        }

        public async Task AddAsync(DeviceDTO device)
        {
            device.CreatedDate = device.UpdatedDate = DateTime.UtcNow;
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeviceDTO device)
        {
            device.UpdatedDate = DateTime.UtcNow;
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

    }
}
