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
    private readonly IFileProvider _fileProvider;

    public DeviceService(AppDbContext appContext, IMapper mapper, UserManager<User> userManager,
        IFileProvider fileProvider)
    {
        _appContext = appContext;
        _mapper = mapper;
        _userManager = userManager;
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

    public async Task<QrCodeModel> GetQrCodeAsync(int id)
    {
        var device = await _appContext.Devices.FindAsync(id);

        if (device != null)
        {
            QREncoder encoder = new()
            {
                ModuleSize = 10
            };
            string path = ("QRcode/" + device?.Id + ".png");
            if (!File.Exists(path))
            {
                encoder.Encode(device.Id.ToString());
                encoder.SaveQRCodeToPngFile("QRcode/" + device?.Id + ".png");
                IFileInfo fileInfo = _fileProvider.GetFileInfo(path);
                var fs = fileInfo.CreateReadStream();
                string contentType = "image/png";
                string downloadName = device?.Id + ".png";
                return new QrCodeModel()
                {
                    Name = downloadName,
                    Type = contentType,
                    Path = fs
                };
            }
            else
            {
                IFileInfo fileInfo = _fileProvider.GetFileInfo(path);
                var fs = fileInfo.CreateReadStream();
                string contentType = "image/png";
                string downloadName = device?.Id + ".png";
                return new QrCodeModel()
                {
                    Name = downloadName,
                    Type = contentType,
                    Path = fs
                };
            }
            
        }

        throw new Exception("Device is not found");
    }

    public async Task<IList<Employee>> GetEmployeesAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return _mapper.Map<IList<Employee>>(users);
    }

    public async Task<string> InventoryAsync(int id, string name)
    {
        var device = await _appContext.Devices.FindAsync(id);
        var user = await _userManager.FindByNameAsync(name);
        if (device != null)
        {
            var inventory = new Inventory()
            {
                CreatedBy = user,
                InventoryDate = DateTime.Today,
                DeviceId = device.Id
            };
            await _appContext.Inventories.AddAsync(inventory);
            await _appContext.SaveChangesAsync();
            return "Inventory is successfully ";
        }

        throw new Exception("Device is not found");
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
        await _appContext.Devices.AddAsync(device);
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