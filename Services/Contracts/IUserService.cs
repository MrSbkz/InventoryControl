using InventoryControl.Data.Entities;
using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IUserService
{
    public Task<IList<UserModel>> GetUsersAsync();

    public Task<User> GetByUserAsync(UserModel model);

    public Task<RegisterResponse> AddUserAsync(RegisterModel model);

    public Task<string> UpdateUserAsync(UserModel model);

    public Task<string> ResetPasswordAsync(User model, string lastPassword, string newPassword);

    public Task<string> DeleteUserAsync(UserModel model);
}