using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IDeviceService
{
    public Task<Page<DeviceDto>> GetDeviceListAsync(int currentPage, int pageSize);

    public Task<DeviceDto> GetDeviceAsync(int id);

    public Task<QrCodeModel> GetDeviceByQrAsync(int id);

    public Task<string> InventoryAsync();

    public Task<string> AddDeviceAsync(RegisterDeviceModel model);

    public Task<string> UpdateDeviceAsync(UpdateDeviceModel model);

    public Task<string> DeleteDeviceAsync(int id);
}