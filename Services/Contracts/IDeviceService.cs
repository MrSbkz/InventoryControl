using InventoryControl.Data.Entities;
using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IDeviceService
{
    public Task<Page<DeviceDto>> GetDevicesAsync(
        string? searchString,
        bool showDecommissionDevice,
        bool showUnassignedDevices,
        int currentPage,
        int pageSize);

    public Task<IList<DeviceDto>> GetDevicesListAsync(
        string searchString,
        bool showDecommissionDevice,
        bool showUnassignedDevices);
    
    public Task<DeviceDto> GetDeviceAsync(int id);
    
    public Task<List<DeviceDto>> GetUserDevicesAsync(string userId);

    public Task<IList<Employee>> GetEmployeesAsync();

    public Task<QrCodeModel> GetQrCodeAsync(int id);

    public Task<string> InventoryAsync(int id, string name);
    public Task<IList<DeviceHistoryDto>> GetDeviceHistoryAsync(int deviceId);

    public Task<DeviceDto> AddDeviceAsync(AddDeviceModel model);

    public Task<DeviceDto> UpdateDeviceAsync(UpdateDeviceModel model);
    
    public Task<DeviceDto> DecommissDeviceAsync(int id, string userName);

  
}