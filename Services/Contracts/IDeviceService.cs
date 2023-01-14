﻿using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IDeviceService
{
    public Task<Page<DeviceDto>> GetDeviceListAsync(int currentPage, int pageSize);

    public Task<DeviceDto> GetDeviceAsync(int id);

    public Task<DeviceDto> GetDeviceByQrAsync();

    public Task<string> InventoryAsync();

    public Task<string> AddDeviceAsync(RegisterDeviceModel model);
}