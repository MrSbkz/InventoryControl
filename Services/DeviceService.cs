using AutoMapper;
using InventoryControl.Data;
using InventoryControl.Data.Entities;
using InventoryControl.Enums;
using InventoryControl.Extensions;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using QRCodeEncoderLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace InventoryControl.Services;

public class DeviceService : IDeviceService
{
    const string ContentType = "image/png";

    private readonly AppDbContext _appContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IFileProvider _fileProvider;
    private readonly IConfiguration _configuration;

    public DeviceService(
        AppDbContext appContext,
        IMapper mapper,
        UserManager<User> userManager,
        IFileProvider fileProvider,
        IConfiguration configuration)
    {
        _appContext = appContext;
        _mapper = mapper;
        _userManager = userManager;
        _fileProvider = fileProvider;
        _configuration = configuration;
    }

    public async Task<Page<DeviceDto>> GetDevicesAsync(
        string? searchString,
        bool showDecommissionDevice,
        bool showUnassignedDevices,
        int currentPage,
        int pageSize)
    {
        var devices = await SearchDevicesAsync(searchString, showDecommissionDevice, showUnassignedDevices);

        return new Page<DeviceDto>()
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalItems = devices.Count,
            Content = _mapper.Map<IList<DeviceDto>>(devices.Skip((currentPage - 1) * pageSize).Take(pageSize)
                .ToList())
        };
    }

    public async Task<IList<DeviceDto>> GetDevicesListAsync(
        string searchString,
        bool showDecommissionDevice,
        bool showUnassignedDevices)
    {
        return _mapper.Map<IList<DeviceDto>>(await SearchDevicesAsync(searchString, showDecommissionDevice,
            showUnassignedDevices));
    }

    public async Task<DeviceDto> GetDeviceAsync(int id)
    {
        var device = await _appContext.Devices.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<DeviceDto>(device);
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
            var fileName = device.Id + ".png";
            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
            }

            var path = "Images/" + fileName;
            var link = string.Format(_configuration.GetSection("QrBasePath").Value, device.Id);
            encoder.Encode(link);
            encoder.SaveQRCodeToPngFile(path);
            var fileInfo = _fileProvider.GetFileInfo(path);
            var fs = fileInfo.CreateReadStream();
            return new QrCodeModel()
            {
                Name = fileName,
                Type = ContentType,
                Path = fs
            };
        }

        throw new Exception("Devices is not found");
    }

    public async Task<IList<Employee>> GetEmployeesAsync()
    {
        var users = await _userManager.Users.Where(x => x.IsActive).ToListAsync();
        return _mapper.Map<IList<Employee>>(users).OrderBy(x => x.FullName).ToList();
    }

    public async Task<string> InventoryAsync(int id, string name)
    {
        var device = await _appContext.Devices.FindAsync(id);
        var user = await _userManager.FindByNameAsync(name);

        if (device == null)
        {
            throw new Exception("Devices is not found");
        }

        if (device.DecommissionDate != null)
        {
            throw new Exception("Cannot inventory decommission device!");
        }

        var inventory = new Inventory()
        {
            CreatedBy = user,
            InventoryDate = DateTime.Now,
            DeviceId = device.Id
        };
        await _appContext.Inventories.AddAsync(inventory);

        await AddDeviceHistoryAsync(DeviceHistoryAction.Inventory, user, device);

        return "Inventory is successfully ";
    }

    public async Task<IList<DeviceHistoryDto>> GetDeviceHistoryAsync(int deviceId)
    {
        var history = await _appContext.DeviceHistories.Where(x => x.DeviceId == deviceId).ToListAsync();

        return _mapper.Map<IList<DeviceHistoryDto>>(history);
    }

    public async Task<DeviceDto> AddDeviceAsync(AddDeviceModel model)
    {
        if (!_userManager.Users.Any(x => x.UserName == model.UserName))
        {
            throw new Exception("User is not found");
        }

        var user = await _userManager.FindByNameAsync(model.UserName);
        var device = new Device
        {
            Name = model.Name,
            RegisterDate = DateTime.Now,
            UserId = user.Id,
            DecommissionDate = null,
        };
        await _appContext.Devices.AddAsync(device);
        await _appContext.SaveChangesAsync();
        await AddDeviceHistoryAsync(DeviceHistoryAction.Assigned, user, device);

        return _mapper.Map<DeviceDto>(device);
    }

    public async Task<DeviceDto> UpdateDeviceAsync(UpdateDeviceModel model)
    {
        var device = await _appContext.Devices.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == model.Id);
        if (device == null)
        {
            throw new Exception("Devices is not found");
        }

        await UpdateDeviceAssignedAsync(device, model);

        if (device.Name != model.Name)
        {
            await AddDeviceHistoryAsync(DeviceHistoryAction.UpdateName, device.User, device, model.Name);
            device.Name = model.Name;
        }

        _appContext.Devices.Update(device);

        await _appContext.SaveChangesAsync();
        return _mapper.Map<DeviceDto>(await _appContext.Devices.FindAsync(model.Id));
    }

    public async Task<DeviceDto> DecommissDeviceAsync(int id, string userName)
    {
        var device = await _appContext.Devices.FindAsync(id);
        if (device?.DecommissionDate != null)
        {
            throw new Exception("Devices already decommissioned");
        }

        if (device != null)
        {
            device.DecommissionDate = DateTime.Now;
            device.UserId = null;
            _appContext.Devices.Update(device);
            await _appContext.SaveChangesAsync();
            var user = await _userManager.FindByNameAsync(userName);
            await AddDeviceHistoryAsync(DeviceHistoryAction.Decommissioned, user, device);
        }

        await _appContext.SaveChangesAsync();
        return _mapper.Map<DeviceDto>(await _appContext.Devices.FindAsync(id));
    }

    private async Task<IList<Device>> SearchDevicesAsync(
        string? searchString,
        bool showDecommissionDevice,
        bool showUnassignedDevices)

    {
        var devices = new List<Device>();

        var search = !string.IsNullOrEmpty(searchString) ? searchString.Replace(" ", "") : string.Empty;

        if (showUnassignedDevices)
        {
            devices = await _appContext.Devices.Include(x => x.User)
                .Where(x => x.UserId == null && x.Name.Contains(search)).ToListAsync();
            return devices;
        }

        var devicesByName = await _appContext.Devices
            .Include(x => x.User)
            .Where(x => x.User != null &&
                        (x.DecommissionDate == null || showDecommissionDevice) &&
                        (x.Name.Contains(search)))
            .ToListAsync();

        devices.AddRange(devicesByName);

        var devicesByAssignedToUserName = await _appContext.Devices
            .Include(x => x.User)
            .Where(x =>
                x.User != null &&
                (x.DecommissionDate == null || showDecommissionDevice) &&
                (x.User.UserName.Contains(search)))
            .ToListAsync();

        devices.AddRange(devicesByAssignedToUserName.Except(devicesByName));

        var devicesByAssignedToFullName = await _appContext.Devices
            .Include(x => x.User)
            .Where(x =>
                x.User != null &&
                (x.DecommissionDate == null || showDecommissionDevice) &&
                ((x.User.FirstName + x.User.LastName).Contains(search) ||
                 (x.User.LastName + x.User.FirstName).Contains(search)))
            .ToListAsync();

        devices.AddRange(devicesByAssignedToFullName.Except(devicesByAssignedToUserName));

        return devices;
    }

    private async Task AddDeviceHistoryAsync(
        DeviceHistoryAction action,
        User? user,
        Device? device,
        string oldName = "")
    {
        var actionString = "";

        switch (action)
        {
            case DeviceHistoryAction.Assigned:
                if (device != null)
                {
                    actionString = string.Format(action.GetAttribute(),
                        device.User?.FirstName + " " + device.User?.LastName + "(" + device.User?.UserName + ")");
                }

                break;

            case DeviceHistoryAction.Inventory:
                if (user != null)
                    actionString = string.Format(action.GetAttribute(),
                        user.FirstName + " " + user.LastName + "(" + user.UserName + ")");
                break;

            case DeviceHistoryAction.UpdateName:
                actionString = string.Format(action.GetAttribute(), device?.Name, oldName);
                break;

            case DeviceHistoryAction.Decommissioned:
                if (device != null)
                    if (user != null)
                        actionString = string.Format(action.GetAttribute(),
                            user.FirstName + " " + user.LastName + "(" + user.UserName + ")");
                break;

            case DeviceHistoryAction.Unassigned:
                actionString = action.GetAttribute();
                break;
        }

        var history = new DeviceHistory
        {
            Action = actionString,
            DeviceId = device!.Id,
            CreatedDate = DateTime.Now,
        };
        _appContext.DeviceHistories.Update(history);
        await _appContext.SaveChangesAsync();
    }

    private async Task UpdateDeviceAssignedAsync(Device device, UpdateDeviceModel model)
    {
        if (device.User?.UserName == model.AssignedTo) return;
        if (string.IsNullOrEmpty(model.AssignedTo))
        {
            await AddDeviceHistoryAsync(DeviceHistoryAction.Unassigned, null, device);
            device.UserId = null;
            device.User = null;
        }
        else
        {
            var user = await _userManager.FindByNameAsync(model.AssignedTo);
            if (!user.IsActive) throw new Exception("User is inactive");
            device.UserId = user.Id;
            device.User = user;
            await AddDeviceHistoryAsync(DeviceHistoryAction.Assigned, user, device);
        }
    }
}