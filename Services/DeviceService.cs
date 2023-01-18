using AutoMapper;
using InventoryControl.Data;
using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using QRCodeEncoderLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace InventoryControl.Services;

public class DeviceService : IDeviceService
{
    private readonly AppDbContext _appContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IFileProvider _fileProvider;

    public DeviceService(AppDbContext appContext, IMapper mapper, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager, IFileProvider fileProvider)
    {
        _appContext = appContext;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _fileProvider = fileProvider;
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

    public async Task<DeviceDto> GetDeviceAsync(int id)
    {
        var device = await _appContext.Devices.FindAsync(id);
        var employee = await _userManager.FindByIdAsync(device?.UserId);
        var deviceDto = _mapper.Map<DeviceDto>(device);
        deviceDto.AssignedTo = _mapper.Map<Employee>(employee);
        return deviceDto;
    }

    public async Task<QrCodeModel> GetDeviceByQrAsync(int id)
    {
        var device = await _appContext.Devices.FindAsync(id);

        if (device != null)
        {
            QREncoder encoder = new()
            {
                ModuleSize = 10
            };
            encoder.Encode(device?.Id.ToString());
            encoder.SaveQRCodeToPngFile("QRcode/" + device.Id + ".png");
            string path = ("QRcode/" + device.Id + ".png");
            IFileInfo fileInfo = _fileProvider.GetFileInfo(path);
            var fs = fileInfo.CreateReadStream();
            string contentType = "image/png";
            string downloadName = device.Id + ".png";
            return new QrCodeModel()
            {
                Name = downloadName,
                Type = contentType,
                Path = fs
            };
        }

        throw new Exception("Device is not found");
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
        if (!_userManager.Users.Any(x => x.UserName == model.UserName))
        {
            throw new Exception("User is not found");
        }

        var device = new Device()
        {
            Name = model.Name,
            RegisterDate = DateTime.Today,
            User = await _userManager.FindByNameAsync(model.UserName),
            UserId = _userManager.FindByNameAsync(model.UserName).Id.ToString(),
            DecommissionDate = null,
        };

        QREncoder encoder = new();
        encoder.Encode(device?.Id.ToString());
        if (device != null)
        {
            encoder.SaveQRCodeToPngFile("QRcode/" + device.Id + ".jpg");
            await _appContext.Devices.AddAsync(device);
        }

        if (device != null) await _appContext.Devices.AddAsync(device);
        await _appContext.SaveChangesAsync();

        return "Device added successfully ";
    }

    public async Task<string> UpdateDeviceAsync(UpdateDeviceModel model)
    {
        var device = await _appContext.Devices.FindAsync(model.Id);
        if (device == null)
        {
            throw new Exception("Device is not found");
        }

        var user = await _userManager.FindByNameAsync(model.AssignedTo.UserName);

        if (user != null)
        {
            device.Name = model.Name;
            device.User = await _userManager.FindByNameAsync(model.AssignedTo.UserName);
            device.UserId = _userManager.FindByNameAsync(model.AssignedTo.UserName).Id.ToString();

            _appContext.Devices.Update(device);

            await _appContext.SaveChangesAsync();
            return "Device update successfully!";
        }

        throw new Exception("User is not found");
    }

    public async Task<string> DeleteDeviceAsync(int id)
    {
        var device = await _appContext.Devices.FindAsync(id);
        if (device?.DecommissionDate != null)
        {
            return "Device already decommissioned";
        }

        if (device != null)
        {
            device.DecommissionDate = DateTime.Today;
            _appContext.Devices.Update(device);
        }

        await _appContext.SaveChangesAsync();

        return "Device decommissioned";
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