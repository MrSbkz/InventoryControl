using InventoryControl.Data.Entities;
using InventoryControl.Models;

namespace InventoryControl.Services.Contracts;

public interface IUserService
{
    public Task<UserModel> GetUsers { get; set; }

    public Task<User> GetByUser { get; set; }

    public Task<User> AddUser { get; set; }

    public Task<User> UpdateUser { get; set; }

    public Task<string> ResetPassword { get; set; }

    public Task<string> DeleteUser { get; set; }
}