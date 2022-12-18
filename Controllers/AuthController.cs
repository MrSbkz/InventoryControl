using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
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
            try
            {
                var token = await _authService.LoginAsync(model);

                return Ok(new Response<LoginResponse>
                {
                    IsSuccess = true,
                    Data = token
                });
            }

            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            try
            {
                var result = await _authService.RegisterAsync(model);
                if (result.Any(s=>s.Contains("must")))
                {
                    return BadRequest(new Response<IList<string>>
                    {
                        IsSuccess = false,
                        Errors =  result
                    });
                }
                return Ok(new Response<IList<string>>
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response<IList<string>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }
    }
}
