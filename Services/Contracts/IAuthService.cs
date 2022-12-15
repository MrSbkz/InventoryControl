using InventoryControl.Data.Entities;
using InventoryControl.Models;

namespace InventoryControl.Services.Contracts
{
    public interface IAuthService
    {
        public Task<LoginResponse> LoginAsync(LoginModel model);

        public Task<AuthResponse> RegisterAsync(RegisterModel model);
    }
}
