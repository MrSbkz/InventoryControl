using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IDeviceService
{
    public Task<Page<DeviceDto>> GetDevicesAsync(
        string searchString,
        bool showDecommissionDevice,
        bool showInActiveDevice,
        int currentPage,
        int pageSize);

    public Task<DeviceDto> GetDeviceAsync(int id);

    public Task<IList<Employee>> GetEmployeesAsync();

    public Task<QrCodeModel> GetQrCodeAsync(int id);

    public Task<string> InventoryAsync(int id, string name);

    public Task<DeviceDto> AddDeviceAsync(AddDeviceModel model);

    public Task<string> UpdateDeviceAsync(UpdateDeviceModel model);

    public Task<string> DecommissDeviceAsync(int id);
}