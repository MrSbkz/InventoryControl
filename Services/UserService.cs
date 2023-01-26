using AutoMapper;
using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventoryControl.Models;
using NuGet.Packaging;

namespace InventoryControl.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Page<UserDto>> GetUsersAsync(int currentPage, int pageSize)
    {
        var users = await _userManager.Users.Where(x => x.IsActive).Skip((currentPage - 1) * pageSize).Take(pageSize)
            .ToListAsync();

        return new Page<UserDto>()
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalItems = _userManager.Users.Count(x => x.IsActive),
            Content = await GetUserDtosAsync(users)
        };
    }

    public async Task<UserDto?> GetUserAsync(string? userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user != null)
        {
            return await GetUserDtoAsync(user);
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

    public async Task<IList<UserDto>> SearchUsersAsync(string searchString)
    {
        var users = new List<User>();

        var usersLastName = await _userManager.Users.Where(x => x.LastName.Contains(searchString)).ToListAsync();

        if (usersLastName != null)
        {
            users.AddRange(usersLastName);
        }

        var usersFirtsNameDto = await _userManager.Users.Where(x => x.FirstName.Contains(searchString))
            .ToListAsync();
        var usersFirtsName = usersFirtsNameDto.Except(usersLastName);

        if (usersFirtsName != null)
        {
            users.AddRange(usersFirtsName);
        }

        var usersUserNameDto = await _userManager.Users
            .Where(x => x.UserName.Contains(searchString))
            .ToListAsync();

        var usersUserName = usersUserNameDto.Except(usersFirtsName);

        if (usersUserName != null)
        {
            users.AddRange(usersUserName);
        }

        return await GetUserDtosAsync(users);
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
}