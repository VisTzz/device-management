using DevicesManagement.Domain.Entities;
using DevicesManagement.Domain.Interfaces;

namespace DevicesManagement.Application
{
    public class DeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task AddOrUpdateDevice(DeviceDTO device)
        {
            var existingDevice = await _deviceRepository.GetByIdAsync(device.Id);
            if (existingDevice == null)
            {
                await _deviceRepository.AddAsync(device);
            }
            else
            {
                existingDevice.DeviceStatus = device.DeviceStatus;
                await _deviceRepository.UpdateAsync(existingDevice);
            }
        }
    }
}
