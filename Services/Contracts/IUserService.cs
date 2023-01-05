using InventoryControl.Data.Entities;
using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IUserService
{
    public Task<IList<UserDTO>> GetUsersAsync();

    public Task<User> GetByUserAsync(UserDTO dto);

    public Task<RegisterResponse> AddUserAsync(RegisterModel model);

    public Task<string> UpdateUserAsync(UserDTO dto);

    public Task<string> ResetPasswordAsync(User model, string lastPassword, string newPassword);

    public Task<string> DeleteUserAsync(UserDTO dto);
}