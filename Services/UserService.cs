using AutoMapper;
using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IDeviceService _deviceService;

    public UserService(UserManager<User> userManager, IMapper mapper, IDeviceService deviceService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _deviceService = deviceService;
    }

    public async Task<Page<UserDto>> GetUsersAsync(
        string? searchString,
        bool showInactiveUsers,
        int currentPage,
        int pageSize)
    {
        var users = await SearchUsersAsync(searchString, showInactiveUsers);

        return new Page<UserDto>
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalItems = users.Count,
            Content = await GetUserDtosAsync(users.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList())
        };
    }

    public async Task<DeivceOfUser?> GetUserAsync(
        string? userName,
        bool showDecommissionDevice,
        int currentPage,
        int pageSize)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user != null)
        {
            var device = await _deviceService.GetDevicesAsync(user.UserName,
                showDecommissionDevice,
                currentPage,
                pageSize);

            return new DeivceOfUser()
            {
                User = await GetUserDtoAsync(user),
                Device = device
            };
        }

        throw new Exception("User is not found");
    }

    public async Task<RegisterResponse> AddUserAsync(RegisterModel model)
    {
        if (_userManager.Users.Any(x => x.UserName == model.UserName))
        {
            throw new Exception("User already exists");
        }

        var user = new User
        {
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            IsActive = true,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return new RegisterResponse
            {
                IsSuccess = false,
                Data = result.Errors.Select(x => x.Description).ToList()
            };
        }

        foreach (var userRole in model.Roles)
        {
            await _userManager.AddToRoleAsync(user, userRole);
        }

        return new RegisterResponse
        {
            IsSuccess = true,
            Data = new List<string> { "User created successfully!" }
        };
    }

    public async Task<string> UpdateUserAsync(UpdateUserModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = model.Roles.Except(userRoles);
            var removedRoles = userRoles.Except(model.Roles);

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _userManager.UpdateAsync(user);
            return "User update successfully!";
        }

        throw new Exception("User is not found");
    }

    public async Task<string> DeleteUserAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user.IsActive)
        {
            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            return "User got inactive";
        }

        return "User already inactive";
    }

    public async Task<string> RestoreUserAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (!user.IsActive)
        {
            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return "User got active";
        }

        return "User already active";
    }


    private async Task<IList<UserDto>> GetUserDtosAsync(List<User> users)
    {
        var result = new List<UserDto>();
        foreach (var user in users)
        {
            result.Add(await GetUserDtoAsync(user));
        }

        return result;
    }

    private async Task<UserDto> GetUserDtoAsync(User user)
    {
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = await _userManager.GetRolesAsync(user);
        return userDto;
    }

    private async Task<IList<User>> SearchUsersAsync(string? searchString, bool showInactiveUsers)
    {
        var users = new List<User>();
        var search = !string.IsNullOrEmpty(searchString) ? searchString.Replace(" ", "") : string.Empty;
        var usersByFullName =
            await _userManager.Users
                .Where(x =>
                    (x.IsActive || showInactiveUsers) &&
                    ((x.FirstName + x.LastName).Contains(search) ||
                     (x.LastName + x.FirstName).Contains(search)))
                .ToListAsync();

        users.AddRange(usersByFullName);

        var usersByUserName = await _userManager.Users
            .Where(x =>
                (x.IsActive || showInactiveUsers) &&
                x.UserName.Contains(search))
            .ToListAsync();

        users.AddRange(usersByUserName.Except(usersByFullName));

        return users;
    }
}