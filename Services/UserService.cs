using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class UserService : IUserService
{
    private UserManager<User> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IList<UserModel>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var usersList = new List<UserModel>();
        foreach (var user in usersList)
        {
            var userDto = user;
            usersList.Add(userDto);
        }

        return usersList;
    }

    public async Task<User> GetByUserAsync(UserModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user != null)
        {
            return user;
        }

        throw new Exception("User is not found");
    }

    public async Task<RegisterResponse> AddUserAsync(RegisterModel model)
    {
        var existingUser = await _userManager.FindByNameAsync(model.UserName);
        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }

        var user = new User
        {
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
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

    public async Task<string> UpdateUserAsync(UserModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user != null)
        {
            user.UserName = model.UserName;
            user.FirstName = model.FistName;
            user.LastName = model.LastName;
            await _userManager.UpdateAsync(user);

            return "User update successfully!";
        }
        else
            throw new Exception("User is not found");
    }

    public async Task<string> ResetPasswordAsync(User model, string lastPassword, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user != null && await _userManager.CheckPasswordAsync(user, lastPassword))
        {
            user.PasswordHash = newPassword;
            return "Password successfully changed";
        }
        else
            throw new Exception("Wrong UserName or password");
    }

    public async Task<string> DeleteUserAsync(UserModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user.IsActive == true)
        {
            user.IsActive = false;
            return "User banned";
        }
        else
            return "User already banned";
    }
}