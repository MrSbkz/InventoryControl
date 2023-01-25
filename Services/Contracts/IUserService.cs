using InventoryControl.Data.Entities;
using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IUserService
{
    public Task<Page<UserDto>> GetUsersAsync(int currentPage, int pageSize);

    public Task<UserDto?> GetUserAsync(string? userName);

    public Task<RegisterResponse> AddUserAsync(RegisterModel model);

    public Task<string> UpdateUserAsync(UpdateUserModel dto);

    public Task<string> DeleteUserAsync(string userName);

    public Task<string> RestoreUserAsync(string userName);
    
    public Task<IList<UserDto>> SearchUsersAsync(string searchString);
}