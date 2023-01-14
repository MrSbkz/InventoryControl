using AutoMapper;
using InventoryControl.Data;
using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace InventoryControl.Services;

public class DeviceService : IDeviceService
{
    private readonly AppDbContext _appContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public DeviceService(AppDbContext appContext, IMapper mapper, UserManager<User> userManager)
    {
        _appContext = appContext;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Page<DeviceDto>> GetDeviceListAsync(int currentPage, int pageSize)
    {
        var devices = await _appContext.Devices.Where(x => x.DecommissionDate == null)
            .Skip((currentPage - 1) * pageSize).Take(pageSize)
            .ToListAsync();

        return new Page<DeviceDto>()
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalItems = _appContext.Devices.Count(x => x.DecommissionDate == null),
            Content = await MapDevice(devices)
        };
    }

    public Task<DeviceDto> GetDeviceAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DeviceDto> GetDeviceByQrAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetEmployeeByDiveceAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<string> InventoryAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddDeviceAsync(RegisterDeviceModel model)
    {
        var device = new Device()
        {
            Name = model.Name,
            RegisterDate = DateTime.Today,
            User = await _userManager.FindByNameAsync(model.UserName),
            UserId = _userManager.FindByNameAsync(model.UserName).Id.ToString(),
            DecommissionDate = null
        };

        await _appContext.Devices.AddAsync(device);

        await _appContext.SaveChangesAsync();

        return "Device created successfully";
    }

    private async Task<IList<DeviceDto>> MapDevice(List<Device> model)
    {
        var result = new List<DeviceDto>();
        foreach (var device in model)
        {
            var employee = await _userManager.FindByIdAsync(device.UserId);
            var deviceDto = _mapper.Map<DeviceDto>(device);
            deviceDto.AssignedTo = _mapper.Map<Employee>(employee);
            result.Add(deviceDto);
        }

        return result;
    }
}