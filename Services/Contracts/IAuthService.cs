﻿using InventoryControl.Models;

namespace InventoryControl.Services.Contracts
{
    public interface IAuthService
    {
        public Task<LoginResponse> LoginAsync(LoginModel model);

        public Task<IList<string>> RegisterAsync(RegisterModel model);
    }
}