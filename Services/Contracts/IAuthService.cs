using InventoryControl.Data.Entities;
using InventoryControl.Models;

namespace InventoryControl.Services.Interface
{
    public interface IAuthService
    {
        public Task<LoginResponse> LoginAsync(User user);
        public Task<AuthResponse> RegisterAsync(RegisterModel model);
    }
}
