using InventoryControl.Models;

namespace InventoryControl.Services.Contracts
{
    public interface IAuthService
    {
        public Task<LoginResponse> LoginAsync(LoginModel model);

        public Task<Response<RegisterModel>> RegisterAsync(RegisterModel model);
    }
}
