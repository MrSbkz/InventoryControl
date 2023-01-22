﻿using AutoMapper;
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

    public async Task<Page<DeviceDto>> GetDevicesAsync(int currentPage, int pageSize)
    {
        var devices = await _appContext.Devices.Include(x => x.User).Where(x => x.DecommissionDate == null)
            .Skip((currentPage - 1) * pageSize).Take(pageSize)
            .ToListAsync();

        return new Page<DeviceDto>()
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalItems = _appContext.Devices.Count(x => x.DecommissionDate == null),
            Content = _mapper.Map<IList<DeviceDto>>(devices)
        };
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
            var path = (device.Id + ".png");
            var getLink = string.Format(_configuration.GetSection("QrBasePath").Value, device.Id);
            encoder.Encode(getLink);
            encoder.SaveQRCodeToPngFile("Images/" + path);
            var fileInfo = _fileProvider.GetFileInfo("Images/" + path);
            var fs = fileInfo.CreateReadStream();
            return new QrCodeModel()
            {
                Name = path,
                Type = ContentType,
                Path = fs
            };
        }

        throw new Exception("Device is not found");
    }

    public async Task<IList<Employee>> GetEmployeesAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return _mapper.Map<IList<Employee>>(users).OrderBy(x => x.FullName).ToList();
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
                InventoryDate = DateTime.Now,
                DeviceId = device.Id
            };
            await _appContext.Inventories.AddAsync(inventory);
            await _appContext.SaveChangesAsync();
            return "Inventory is successfully ";
        }

        throw new Exception("Device is not found");
    }

    public async Task<string> AddDeviceAsync(AddDeviceModel model)
    {
        if (!_userManager.Users.Any(x => x.UserName == model.UserName))
        {
            throw new Exception("User is not found");
        }

        var device = new Device
        {
            Name = model.Name,
            RegisterDate = DateTime.Now,
            UserId = (await _userManager.FindByNameAsync(model.UserName)).Id,
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

        if (!string.IsNullOrEmpty(model.AssignedTo))
        {
            device.UserId = (await _userManager.FindByNameAsync(model.AssignedTo)).Id;
        }

        device.Name = model.Name;


        _appContext.Devices.Update(device);

        await _appContext.SaveChangesAsync();
        return "Device update successfully!";
    }

    public async Task<string> DecommissDeviceAsync(int id)
    {
        var device = await _appContext.Devices.FindAsync(id);
        if (device?.DecommissionDate != null)
        {
            return "Device already decommissioned";
        }

        if (device != null)
        {
            device.DecommissionDate = DateTime.Now;
            _appContext.Devices.Update(device);
        }

        await _appContext.SaveChangesAsync();

        return "Device decommissioned";
    }
}