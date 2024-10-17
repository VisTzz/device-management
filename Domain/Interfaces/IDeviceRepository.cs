using DevicesManagement.Domain.Entities;

namespace DevicesManagement.Domain.Interfaces
{
    public interface IDeviceRepository
    {
        Task<DeviceDTO> GetByIdAsync(int id);
        Task AddAsync(DeviceDTO device);
        Task UpdateAsync(DeviceDTO device);

    }
}
