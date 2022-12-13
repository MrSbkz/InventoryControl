using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public AuthController(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Ok(await _authService.LoginAsync(user));
            }
            return Unauthorized("Wrong Login or password");
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
             
            return result.Status.Equals("Error",StringComparison.OrdinalIgnoreCase)
                ? BadRequest(result)
                : Ok(result);
        }
    }
}
