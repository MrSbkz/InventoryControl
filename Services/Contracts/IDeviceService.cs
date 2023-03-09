using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IDeviceService
{
    public Task<Page<DeviceDto>> GetDevicesAsync(string searchString,
        bool showDecommissionDevice,
        int currentPage,
        int pageSize);

    public Task<DeviceDto> GetDeviceAsync(int id);

    public Task<IList<Employee>> GetEmployeesAsync();

    public Task<QrCodeModel> GetQrCodeAsync(int id);

    public Task<string> InventoryAsync(int id, string name);

    public Task<DeviceDto> AddDeviceAsync(AddDeviceModel model);

    public Task<DeviceDto> UpdateDeviceAsync(UpdateDeviceModel model);

    public Task<DeviceDto> DecommissDeviceAsync(int id, string userName);
}