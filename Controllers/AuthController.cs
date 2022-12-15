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
                var user = await _authService.LoginAsync(model);
                if (user!=null)
                {
                    return Ok(new Response<LoginResponse>
                    {
                        IsSuccess= true,
                        Data = user
                    });
                }
                else
                {
                    return Unauthorized(new Response<LoginResponse>
                    {
                        IsSuccess= false,
                        Errors = new List<string> { "Wrong UserName or password" }
                    });
                }

            }
            catch (Exception e)
            {
                return Unauthorized(new Response<LoginResponse>
                {
                    IsSuccess= false,
                    Errors = new List<string> { e.ToString() }
                });
            }


        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            Response<LoginResponse> response = new Response<LoginResponse>();
            try
            {
                var result = await _authService.RegisterAsync(model);
                return result.Status.Equals("Error", StringComparison.OrdinalIgnoreCase)
                ? BadRequest(new Response<AuthResponse>
                {
                    IsSuccess= false,
                    Errors = new List<string> { result.ToString() }
                })
                : Ok(new Response<AuthResponse>
                {
                    IsSuccess= true,
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response<AuthResponse>
                {
                    IsSuccess= false,
                    Errors = new List<string> { e.ToString() }
                });
            }



        }
    }
}
